using UnityEngine;
using UnityEngine.AI;

public class BeeController : EntityBase
{

    public float movementSpeed = 5;

    private NavMeshAgent navMeshAgent;

    public Vector3 patrolCurrentPos;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;

    }

    private void OnEnable()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed= movementSpeed;
    }

    void Update()
    {
        if (navMeshAgent)
        {
            SetPatrolPoint(navMeshAgent,gameManager.mapLimitX,gameManager.mapLimitZ);
        }
    }

    private void FixedUpdate()
    {
        LimitBounds(transform, gameManager.mapLimitX, gameManager.mapLimitY, gameManager.mapLimitZ);
    }



    
}
