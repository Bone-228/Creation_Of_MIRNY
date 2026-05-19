using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private Ragdoll_debug ragdoll;
    private AiAgent agent;
    [SerializeField] private NavMeshAgent navMeshAgent;
    private UIHealthbar healthbar;

    // Sector reference (IMPORTANT: assigned externally or via trigger)
    public enemy_sector_checker sectorChecker;

    public bool isDead = false;

    // Store the original speed to restore when HP >= 35%
    private float originalNavSpeed = 0f;
    private const float lowHealthSpeed = 3.5f;
    private const float lowHealthThreshold = 0.35f;

    void Start()
    {
        agent = GetComponent<AiAgent>();
        currentHealth = maxHealth;
        ragdoll = GetComponent<Ragdoll_debug>();
        healthbar = GetComponentInChildren<UIHealthbar>();

        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        if (navMeshAgent != null)
        {
            originalNavSpeed = navMeshAgent.speed;
        }

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidBodies)
        {
            HitBox hitBox = rb.gameObject.AddComponent<HitBox>();
            hitBox.enemyHealth = this;
        }
    }

    void Update()
    {
        EnforceSpeedByHealth();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (healthbar != null)
        {
            healthbar.setHealthBarPercentage(currentHealth / maxHealth);
        }

        EnforceSpeedByHealth();

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void EnforceSpeedByHealth()
    {
        if (navMeshAgent == null || maxHealth <= 0f)
            return;

        float hpPercent = currentHealth / maxHealth;

        if (hpPercent < lowHealthThreshold)
        {
            if (!Mathf.Approximately(navMeshAgent.speed, lowHealthSpeed))
            {
                navMeshAgent.speed = lowHealthSpeed;
            }
        }
        else
        {
            if (!Mathf.Approximately(navMeshAgent.speed, originalNavSpeed))
            {
                navMeshAgent.speed = originalNavSpeed;
            }
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        currentHealth = 0f;

        // Notify sector ONLY if assigned
        if (sectorChecker != null)
        {
            sectorChecker.EnemyDied(this);
        }

        // Switch AI state
        if (agent != null && agent.stateMachine != null)
        {
            agent.stateMachine.ChangeState(AiStateID.Death);
        }

        // Optional cleanup delay (prevents instant removal issues)
        Destroy(gameObject, 5f);
    }
}