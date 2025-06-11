using UnityEngine;
using UnityEngine.AI;

public class EntityBase : MonoBehaviour
{
    protected void LimitBounds(Transform target,float maxX, float maxY, float maxZ)
    {
        //Debug.Log($"Limits X:{maxX} Y:{maxY} Z:{maxZ}");

        float clampedX = Mathf.Clamp(target.position.x, -maxX, maxX);
        float clampedZ = Mathf.Clamp(target.position.z, -maxZ, maxZ);
        float clampedY = Mathf.Clamp(target.position.y, -1, maxY);

        if (target.position.x != clampedX || target.position.z != clampedZ)
        {
            target.position = new Vector3(clampedX, clampedY, clampedZ);
          
        }
    }

    protected Vector3 ClampToCameraView(Vector3 worldPos)
    {
        Camera activeCam = Camera.main;
        Vector3 viewportPoint = activeCam.WorldToViewportPoint(worldPos);

        viewportPoint.x = Mathf.Clamp01(viewportPoint.x);
        viewportPoint.y = Mathf.Clamp01(viewportPoint.y);

        Vector3 clampedWorldPos = activeCam.ViewportToWorldPoint(viewportPoint);

        clampedWorldPos.y = transform.position.y;

        return clampedWorldPos;
    }

    protected void SetPatrolPoint(NavMeshAgent navMesh, float maxX, float maxZ )
    {

        if (navMesh.velocity.magnitude <= 0.1f)
        {
            Vector3 patrolPoint = new Vector3(Random.Range(-maxX,
                maxX), transform.position.y,
                Random.Range(-maxZ,
                maxZ));

            navMesh.SetDestination(patrolPoint);
        }
    }

    protected void ChaseTarget(NavMeshAgent navMeshAgent, Vector3 target)
    {
        navMeshAgent.SetDestination(target);
        transform.LookAt(target);
    }

    protected void PushBack(GameObject target, Vector3 direction, float force)
    {
       if (target.TryGetComponent<Rigidbody>(out Rigidbody rb)){
            rb.AddForce(direction * force,ForceMode.Impulse);
            //Debug.Log("Pushing back");
        }
    }

    

}
