using UnityEngine;
using UnityEngine.AI;

public class boss_movement : MonoBehaviour
{
    [Header("Movement")]
    public float wanderRadius = 100f;
    public float reachDistance = 2f;

    private NavMeshAgent agent;
    private Vector3 currentTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PickNewDestination();
    }

    void Update()
    {
        if (agent == null)
            return;

        // If close enough to target, pick a new one
        if (!agent.pathPending && agent.remainingDistance <= reachDistance)
        {
            PickNewDestination();
        }
    }

    void PickNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            currentTarget = hit.position;
            agent.SetDestination(currentTarget);
        }
    }
}