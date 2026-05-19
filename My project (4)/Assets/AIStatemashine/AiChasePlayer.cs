using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayer : AiState
{
    public float maxTime = 0.0f;
    float timer = 0.0f;

    public void Enter(AiAgent agent)
    {
        if (agent != null && agent.navMeshAgent != null)
        {
            // ✅ unified range
            agent.navMeshAgent.stoppingDistance = agent.attackRange;
            agent.navMeshAgent.autoBraking = true;
            agent.navMeshAgent.isStopped = false;
        }

        timer = 0.0f;
    }

    public void Exit(AiAgent agent) { }

    public AiStateID GetID()
    {
        return AiStateID.Chase;
    }

    public void Update(AiAgent agent)
    {
        if (agent == null || agent.player == null) return;
        if (!agent.enabled || agent.navMeshAgent == null) return;

        float attackRange = agent.attackRange;

        Vector3 toPlayer = agent.player.position - agent.transform.position;

        // ✅ STOP + ATTACK at SAME DISTANCE
        if (toPlayer.sqrMagnitude <= attackRange * attackRange)
        {
            agent.navMeshAgent.isStopped = true;
            agent.stateMachine.ChangeState(AiStateID.Attack);
            return;
        }

        // keep moving
        agent.navMeshAgent.isStopped = false;
        agent.navMeshAgent.destination = agent.player.position;

        timer -= Time.deltaTime;

        if (timer < 0.0f)
        {
            timer = Mathf.Max(0.0f, maxTime);
        }
    }
} 