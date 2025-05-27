using UnityEngine;
using UnityEngine.AI;


public class CrocodileController : EntityBase
{
    public float movementSpeed = 10;

    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;

    }

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed;

        try
        {
            playerTransform = GameObject.Find("Player").transform;

        }
        catch (System.Exception)
        {

            playerTransform= null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LimitBounds(transform, gameManager.mapLimitX, gameManager.mapLimitY, gameManager.mapLimitZ);
        if (playerTransform)
        {
            ChaseTarget(navMeshAgent,playerTransform.position);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //GAME OVER
        }
    }
}

