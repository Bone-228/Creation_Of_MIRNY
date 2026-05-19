using System.Collections.Generic;
using UnityEngine;

public class AiAttack : AiState
{
    public float cooldownDuration = 1.0f;
    public float damageAmount = 10.0f;

    private readonly Dictionary<AiAgent, float> _cooldownRemaining = new();
    private readonly Dictionary<AiAgent, Enemy> _enemyComponent = new();
    private readonly Dictionary<AiAgent, bool> _canAttack = new();

    public void Enter(AiAgent agent)
    {
        if (agent == null) return;

        _cooldownRemaining[agent] = 0f;
        _canAttack[agent] = true;

        if (agent.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _enemyComponent[agent] = enemy;
        }
        else
        {
            _enemyComponent.Remove(agent);
        }
    }

    public void Exit(AiAgent agent)
    {
        if (agent == null) return;

        _cooldownRemaining.Remove(agent);
        _enemyComponent.Remove(agent);
        _canAttack.Remove(agent);
    }

    public AiStateID GetID()
    {
        return AiStateID.Attack;
    }

    public void Update(AiAgent agent)
    {
        if (agent == null || agent.player == null) return;

        float distance = Vector3.Distance(agent.transform.position, agent.player.position);
        bool canSeePlayer = distance <= agent.sightSpotDistance;
        bool playerInAttackZone = distance <= agent.attackRange;

        // Ensure dictionaries are initialized
        if (!_cooldownRemaining.TryGetValue(agent, out float currentCooldown)) _cooldownRemaining[agent] = currentCooldown = 0f;
        if (!_canAttack.TryGetValue(agent, out bool canAttack)) _canAttack[agent] = canAttack = true;
        if (!_enemyComponent.TryGetValue(agent, out Enemy enemy) || enemy == null)
        {
            agent.TryGetComponent<Enemy>(out enemy);
            if (enemy != null)
                _enemyComponent[agent] = enemy;
        }

        // Debug log to trace why StartAttack might not trigger
        Debug.Log($"[AiAttack] '{agent.name}' Update: canSeePlayer={canSeePlayer}, playerInAttackZone={playerInAttackZone}, _canAttack={_canAttack[agent]}, EnemyAssigned={enemy != null}");

        // Update cooldown
        float remaining = Mathf.Max(0f, currentCooldown - Time.deltaTime);
        _cooldownRemaining[agent] = remaining;
        _canAttack[agent] = remaining <= 0f;

        // Inform Enemy whether player is in range
        if (enemy != null)
            enemy.SetPlayerInRange(playerInAttackZone);

        // Switch to Chase if player is seen but not in attack range
        if (canSeePlayer && !playerInAttackZone)
        {
            _canAttack[agent] = false;
            _cooldownRemaining[agent] = cooldownDuration;

            if (agent.stateMachine != null)
                agent.stateMachine.ChangeState(AiStateID.Chase);

            return;
        }

        // ATTACK LOGIC: guaranteed to call StartAttack if conditions met
        if (canSeePlayer && playerInAttackZone && _canAttack[agent])
        {
            if (enemy == null)
            {
                Debug.LogWarning($"[AiAttack] '{agent.name}' cannot attack because Enemy component is missing!");
                return;
            }

            Debug.Log($"[AiAttack] '{agent.name}' is ATTACKING the player!");
            enemy.StartAttack();

            _cooldownRemaining[agent] = cooldownDuration;
            _canAttack[agent] = false;
        }
    }
}