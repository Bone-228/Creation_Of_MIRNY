using System.Collections;
using UnityEngine;

public class AiDeath : AiState
{
    private const float DeathDelay = 1.5f;

    public void Enter(AiAgent agent)
    {
        
        if (agent == null)
            return;

        agent.ragdollDebug.EnableRagdoll();
        agent.navMeshAgent.speed = 0f;
        agent.healthBar.gameObject.SetActive(false);

        GiveMirium(agent);
        GiveScraps(agent);
        agent.StartCoroutine(DeleteAfterDelay(agent));
    }

    public void Exit(AiAgent agent) { }

    public AiStateID GetID()
    {
        return AiStateID.Death;
    }

    public void Update(AiAgent agent) { }

    private void GiveMirium(AiAgent agent)
    {
        if (agent.player == null) return;

        miriumPlayerCollector collector =
            agent.player.GetComponent<miriumPlayerCollector>();

        if (collector != null)
        {
            collector.AddMirium(agent.miriumValue);
        }
    }
    private void GiveScraps(AiAgent agent)
    {
        if (agent.player == null) return;
        ScrapCollector collector =
            agent.player.GetComponent<ScrapCollector>();
        if (collector != null)
        {
            collector.AddScraps(agent.scrapsToGive);
        }
    }   
    private IEnumerator DeleteAfterDelay(AiAgent agent)
    {
        yield return new WaitForSeconds(DeathDelay);

        if (agent != null)
            Object.Destroy(agent.gameObject);
    }
}