using System.Reflection;
using UnityEngine;

public class AiIdle : AiState
{
    public void Enter(AiAgent agent)
    {
        
    }

    public void Exit(AiAgent agent)
    {
       
    }

    public AiStateID GetID()
    {
        return AiStateID.Idle;
    }

    public void Update(AiAgent agent)
    {
        if (agent == null || agent.player == null)
        {
            return;
        }

        Vector3 playerDirection = agent.player.position - agent.transform.position;
        float distSqr = playerDirection.sqrMagnitude;
        float sightSqr = agent.sightSpotDistance * agent.sightSpotDistance;
        if (distSqr > sightSqr)
        {
            return;
        }

        Vector3 agentsDirection = agent.transform.forward;
        playerDirection.Normalize();
        float dot = Vector3.Dot(agentsDirection, playerDirection);

        if (dot > 0.0f) 
        { 
            agent.stateMachine.ChangeState(AiStateID.Chase);
        }
    }
}
