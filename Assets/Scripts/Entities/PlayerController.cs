using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityBase
{
    [HideInInspector]
    public float horizontalInput;
    public float verticalInput;

    private PlayerControls playerControls;
    public Vector2 direction;
    public int maxMovementSpeed = 5;
    private int currentSpeed;

    [Range(0f,1f)]
    public float movementAcceleration = 0.2f;

    public float turnSpeed = 500f;
    private Vector3 moveDirection;

    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;

    private PlayerInput playerInput;
    private InputAction pauseGameAction;

    private GameManager gameManager;


    private bool isInvulnerable;
    public bool IsInvulnerable
    {
        get { return isInvulnerable; }
    }
    float invulnerableTime = 3f;
    float invulnerableTimer = 0f;

    public float pushForce = 5f;

    [Range(0.1f, 4f)]
    public float powerUpMultiplier = 2f;

    [Range(1f,10f)]
    public float powerUpTime = 5f;

    private bool powerUpActive;

    private Transform modelTransform;
    private Transform modelPlaceholder;
    private Transform basketTransform;
    private void Awake()
    {

        playerControls = new();

        gameManager = GameManager.Instance;
        playerInput = GetComponent<PlayerInput>();

        modelTransform = transform.Find("Model");
        if (modelTransform)
        {
            basketTransform = transform.Find("Basket");
            if (basketTransform)
            {
                modelPlaceholder = basketTransform.Find("ModelPlaceholder");
            }
        }

      

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
        _meshRenderer = modelTransform.GetComponent<MeshRenderer>();

        invulnerableTimer = invulnerableTime;

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

        if (modelPlaceholder && modelTransform)
        {
            modelTransform.position = modelPlaceholder.position;
        }

        if (isInvulnerable)
        {
            invulnerableTimer -= Time.deltaTime;

            StartCoroutine("blinkRenderer");

            if (invulnerableTimer <= 0)
            {
                isInvulnerable = false;
                _meshRenderer.enabled = true;
                invulnerableTimer = invulnerableTime;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp") && powerUpActive == false)
        {
            if (!powerUpActive)
            {
                StartCoroutine(PowerUpBasket(basketTransform, powerUpMultiplier, powerUpTime));

            }
        }
    }

    public IEnumerator PowerUpBasket(Transform basket, float multiplier, float time)
    {
        powerUpActive = true;

        Vector3 originalScale = basket.localScale;

        basket.localScale = new Vector3(
            x: basket.localScale.x * multiplier,
            y: basket.localScale.y,
            z: basket.localScale.z * multiplier);

        yield return new WaitForSeconds(time);
        powerUpActive = false;
        basket.localScale = originalScale;

        yield return null;
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
