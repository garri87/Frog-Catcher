using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityBase
{

    private PlayerControls playerControls;
    public Vector2 direction;

    public int maxMovementSpeed = 5;
    private int currentSpeed;
    [Range(0f,1f)]
    public float movementAcceleration = 0.2f;
    public float turnSpeed = 500f;

    public float horizontalInput;
    public float verticalInput;

    private Rigidbody _rigidbody;
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


    private PlayerInput playerInput;

    private InputAction pauseGameAction;

    private void Awake()
    {

        playerControls = new();

        gameManager = GameManager.Instance;
        playerInput = GetComponent<PlayerInput>();

    }

    private void OnEnable()
    {
        pauseGameAction = playerInput.actions["Pause"];
        pauseGameAction.started += gameManager.PauseGame;
        playerControls.Enable();
    }


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();

        timer = invulnerableTime;

    }

    void FixedUpdate()
    {
        direction = playerControls.PlayerActions.Movement.ReadValue<Vector2>();

        horizontalInput = playerControls.PlayerActions.Movement.ReadValue<Vector2>().x;
        verticalInput = playerControls.PlayerActions.Movement.ReadValue<Vector2>().y;

        moveDirection = new Vector3(horizontalInput, 0, verticalInput);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }


        Vector3 movement = moveDirection * maxMovementSpeed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(_rigidbody.position + movement);

        LimitBounds(transform, gameManager.mapLimitX, gameManager.mapLimitY, gameManager.mapLimitZ);

    }


    void Update()
    {

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

    private void OnDisable()
    {
        pauseGameAction.started -= gameManager.PauseGame;
        playerControls.Disable();

    }

    public IEnumerator blinkRenderer()
    {
        _meshRenderer.enabled = !_meshRenderer.enabled;
        yield return new WaitForSeconds(0.5f);
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

    

}
