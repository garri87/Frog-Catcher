using UnityEngine;

public class FrogController : EntityBase
{
    public float jumpForce = 5f;          
    public float maxJumpInterval = 2f;
    private float jumpInterval;
    public float jumpHeight = 10f;        
    public float forwardMultiplier = 1.5f;

    private Rigidbody rb;
    private bool isGrounded;
    public bool IsGrounded
    {
        get { return isGrounded; }
    }
    private float timeSinceLastJump;
   

    private bool canJump;

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

        LimitBounds(transform,gameManager.mapLimitX,gameManager.mapLimitY,gameManager.mapLimitZ);

        if(transform.position.y >= gameManager.mapLimitY)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

        if (canJump)
        {
            // Generar una dirección de salto aleatoria con un ángulo fijo de 80°
            float randomAngle = Random.Range(0f, 360f); // Ángulo aleatorio alrededor del eje Y
            Vector3 jumpDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;

            // Rotar la rana hacia la dirección del salto
            transform.rotation = Quaternion.LookRotation(jumpDirection);

            Jump(jumpDirection);
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

    void Jump(Vector3 direction)
    {
        Vector3 jumpVector = direction * jumpForce * forwardMultiplier;

        jumpVector.y = jumpHeight;

        rb.AddForce(jumpVector, ForceMode.Impulse);

        canJump = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpInterval = Random.Range(0, maxJumpInterval);

        }

        if(collision.gameObject.CompareTag("Player") 
            || collision.gameObject.CompareTag("Frog") || collision.gameObject.CompareTag("Basket"))
        {
            canJump = true;
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
