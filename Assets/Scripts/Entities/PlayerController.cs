using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityBase
{
    public int maxMovementSpeed = 5;
    private int currentSpeed;
    [Range(0f,1f)]
    public float movementAcceleration = 0.2f;
    public float turnSpeed = 500f;

    public float horizontalInput;
    public float verticalInput;

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private MeshRenderer _meshRenderer;

    private Vector3 moveDirection;

    private bool isInvulnerable;
    public bool IsInvulnerable
    {
        get { return isInvulnerable; }
    }

    float invulnerableTime = 3f;
    float timer = 0f;

    public float pushForce = 5f;


    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;

    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();

        timer = invulnerableTime;

    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            // Convertir la posición del toque en una dirección de movimiento
            moveDirection = new Vector3(touchPosition.x - Screen.width / 2, 0, touchPosition.y - Screen.height / 2).normalized;
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        }

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }


        if (isInvulnerable)
        {
            timer -= Time.deltaTime;

            StartCoroutine("blinkRenderer");

            if (timer <= 0)
            {
                isInvulnerable = false;
                _meshRenderer.enabled = true;
                timer = invulnerableTime;
            }
        }

    }

    public IEnumerator blinkRenderer()
    {
        _meshRenderer.enabled = !_meshRenderer.enabled;
        yield return new WaitForSeconds(0.5f);
    }

    void FixedUpdate()
    {
        LimitBounds(transform, gameManager.mapLimitX, gameManager.mapLimitY, gameManager.mapLimitZ);

        Vector3 movement = moveDirection * maxMovementSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }


    private void LoseFrogs(float percentaje)
    {
        int catchedFrogs = gameManager.catchedFrogs;
        if (catchedFrogs > 0)
        {
            float result = catchedFrogs - (catchedFrogs * percentaje / 100f);

            if (result < 1)
            {
                result = 0;
            }
            gameManager.catchedFrogs = Mathf.RoundToInt(result);
            GameObject player = GameObject.Find("Player");
            for (int i = 0; i < Mathf.RoundToInt(result); i++)
            {
                gameManager.InstantiateFrog(player.transform.position);
            }
            Debug.Log("Losed frogs, total: " + gameManager.catchedFrogs);
            Debug.Log("Result: " + result);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bee"))
        {
            if (!isInvulnerable)
            {
                PushBack(this.gameObject, transform.position + Vector3.back, pushForce);
                Debug.Log("Player touched by Bee");
                LoseFrogs(50f);
                isInvulnerable = true;
            }
        }

        if (collision.gameObject.CompareTag("Crocodile"))
        {
            gameManager.GameOver();
        }
    }

}
