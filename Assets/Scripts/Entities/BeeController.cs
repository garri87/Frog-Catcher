using UnityEngine;
using UnityEngine.AI;

public class BeeController : EntityBase
{

    public float movementSpeed = 5;

    private GameObject bee;

    private NavMeshAgent navMeshAgent;

    public float yAmp = 1;

    private Transform player;

    private float yPos;

    public Vector3 patrolCurrentPos;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;

    }

    private void OnEnable()
    {
        bee = transform.GetChild(0).gameObject;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed= movementSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {       

        yPos = bee.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        FlyingEffect();
        if (navMeshAgent)
        {
            SetPatrolPoint(navMeshAgent,gameManager.mapLimitX,gameManager.mapLimitZ);
        }
    }

    private void FixedUpdate()
    {
        LimitBounds(transform, gameManager.mapLimitX, gameManager.mapLimitY, gameManager.mapLimitZ);

    }

    private void FlyingEffect()
    {

        bee.transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * movementSpeed/2) * yAmp + yPos, transform.position.z);

    }

    
}
