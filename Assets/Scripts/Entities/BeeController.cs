using UnityEngine;
using UnityEngine.AI;

public class BeeController : EntityBase
{

    public float movementSpeed = 5;

    private NavMeshAgent navMeshAgent;

    public Vector3 patrolCurrentPos;

    private GameManager gameManager;

    public GameObject wingsGO;

    private Quaternion[] wingRots;


    private void Awake()
    {
        gameManager = GameManager.Instance;

    }

    private void OnEnable()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed= movementSpeed;
        wingRots = new Quaternion[wingsGO.transform.childCount];
        for (int i = 0; i < wingsGO.transform.childCount; i++)
        {
            wingRots[i] = wingsGO.transform.GetChild(i).localRotation;
        }
    }

    void Update()
    {
        if (navMeshAgent)
        {
            SetPatrolPoint(navMeshAgent,gameManager.mapLimitX,gameManager.mapLimitZ);
            AnimateWings(movementSpeed * 5);
        }
    }

    private void FixedUpdate()
    {
        LimitBounds(transform, gameManager.mapLimitX, gameManager.mapLimitY, gameManager.mapLimitZ);
    }


    private void AnimateWings(float speed)
    {

        for (int i = 0; i < wingsGO.transform.childCount; i++)
        {
            Transform wing = wingsGO.transform.GetChild(i);

            float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
            float angle = Mathf.Lerp(0, 60, t);

            wing.localRotation = wingRots[i] * Quaternion.AngleAxis(angle, Vector3.forward);

        }
    }

    
}
