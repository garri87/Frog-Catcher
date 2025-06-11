using UnityEngine;

public class FrogController : EntityBase
{
    [Header("Jump Attributes")]
    public float jumpForce = 5f;
    public float jumpAngle = 70f;
    public float jumpHeight = 10f;

    private Vector3 jumpDirection;

    private Quaternion toRotation;
    public float maxJumpInterval = 2f;
    private float jumpInterval;
        
    [Header("Movement")]
    public float turnSpeed = 500f;

    private Rigidbody rb;
    private bool isGrounded;

    public bool IsGrounded
    {
        get { return isGrounded; }
    }
    private float timeSinceLastJump;

    [SerializeField]
    private bool canJump, catchable;
 

    public bool frogCollision;
    public bool playerCollision;


    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeSinceLastJump = maxJumpInterval;
    }

    private void OnEnable()
    {
        LimitBounds(transform, gameManager.mapLimitX, gameManager.mapLimitY, gameManager.mapLimitZ);
    }

    private void FixedUpdate()
    {
        //LimitBounds(transform,gameManager.mapLimitX,gameManager.mapLimitY,gameManager.mapLimitZ);

        Vector3 velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z) * Time.deltaTime;

        Vector3 newPosition = transform.position + velocity;
        transform.position = ClampToCameraView(newPosition);

        //Limit rigidbody Y velocity if is out of bounds
        if (transform.position.y >= gameManager.mapLimitY)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

        //Rotate towards the next jump position
        toRotation = Quaternion.LookRotation(jumpDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

        if (canJump)
        {
            Jump();
        }
    }

    

    void Update()
    {
        if (isGrounded)
        {           
            timeSinceLastJump += Time.deltaTime;
        }

        if (timeSinceLastJump >= jumpInterval && isGrounded)
        {
           
            canJump = true;

            timeSinceLastJump = 0f;
   
        }

    }

    void Jump()
    {
        float angleInRad = jumpAngle * Mathf.Deg2Rad;

        float randomRot = Random.Range(0.001f, 360f);
            
        jumpDirection = Quaternion.Euler(0, randomRot, 0) * Vector3.forward;

        Vector3 jumpVector = new Vector3(jumpDirection.x * Mathf.Cos(angleInRad),
                                         Mathf.Sin(angleInRad), jumpDirection.z * Mathf.Cos(angleInRad)) * jumpForce;

        rb.AddForce(jumpVector, ForceMode.Impulse);

        if (rb.velocity.y > jumpForce)
        {

            rb.velocity = new Vector3(rb.velocity.x,jumpForce,rb.velocity.z);
        }

        canJump = false;
        jumpInterval = Random.Range(1, maxJumpInterval);

    }


    private bool IsOutOfBounds(Vector3 position)
    {
        // Comprobar si la posición supera los límites definidos en el GameManager
        if (position.x < -gameManager.mapLimitX
            || position.x > gameManager.mapLimitX
            || position.z < -gameManager.mapLimitZ
            || position.z > gameManager.mapLimitZ)
        {
            return false;
        }

        return true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            catchable = false;
        }

        if(collision.gameObject.CompareTag("Player") 
            || collision.gameObject.CompareTag("Frog") || collision.gameObject.CompareTag("Basket"))
        {
            canJump = true;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCollision= true;
        }
        else
        {
            playerCollision = false;

        }


        if (collision.gameObject.CompareTag("Frog"))
        {
            frogCollision = true;
        }
        else
        {
            frogCollision = false;
        }

        if (collision.gameObject.CompareTag("Basket"))
        {
            catchable = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
