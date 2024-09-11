using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityBase : MonoBehaviour
{
    protected void LimitBounds(Transform target, float maxX, float maxY, float maxZ)
    {
        //Debug.Log($"Limits X:{maxX} Y:{maxY} Z:{maxZ}");


        float clampedX = Mathf.Clamp(target.position.x, -maxX, maxX);
        float clampedZ = Mathf.Clamp(target.position.z, -maxZ, maxZ);
        float clampedY = Mathf.Clamp(target.position.y, 0, maxY);

        if (target.position.x != clampedX || target.position.z != clampedZ)
        {
            target.position = new Vector3(clampedX, clampedY, clampedZ);
        }
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
