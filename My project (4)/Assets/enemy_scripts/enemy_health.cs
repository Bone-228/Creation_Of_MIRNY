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

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    private UIHealthbar healthbar;

    // Sector reference
    public enemy_sector_checker sectorChecker;

    public bool isDead = false;

    // Movement speed settings
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

        // Add hitboxes to all ragdoll rigidbodies
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

    // ───────────────────────── DAMAGE ─────────────────────────

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // Update UI healthbar
        if (healthbar != null)
        {
            healthbar.setHealthBarPercentage(
                currentHealth / maxHealth
            );
        }

        EnforceSpeedByHealth();

        // Death check
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // ───────────────────────── HEALTH-BASED SPEED ─────────────────────────

    private void EnforceSpeedByHealth()
    {
        if (navMeshAgent == null || maxHealth <= 0f)
            return;

        float hpPercent = currentHealth / maxHealth;

        // Low HP speed
        if (hpPercent < lowHealthThreshold)
        {
            if (!Mathf.Approximately(navMeshAgent.speed, lowHealthSpeed))
            {
                navMeshAgent.speed = lowHealthSpeed;
            }
        }
        else
        {
            // Restore original speed
            if (!Mathf.Approximately(navMeshAgent.speed, originalNavSpeed))
            {
                navMeshAgent.speed = originalNavSpeed;
            }
        }
    }

    // ───────────────────────── DEATH ─────────────────────────

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        currentHealth = 0f;

        // Notify sector
        if (sectorChecker != null)
        {
            sectorChecker.EnemyDied(this);
        }

        // Change AI state
        if (agent != null && agent.stateMachine != null)
        {
            agent.stateMachine.ChangeState(AiStateID.Death);
        }

        // ───────────────────────── LIFESTEALER ─────────────────────────

        ModifierManager modifierManager =
            FindObjectOfType<ModifierManager>();

        if (modifierManager != null)
        {
            modifierManager.OnEnemyKilled();
        }

        // Cleanup
        Destroy(gameObject, 5f);
    }
}