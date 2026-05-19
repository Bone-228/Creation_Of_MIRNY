using UnityEngine;

public class EnemyHeavy : Enemy
{
    const string AttackStateName = "attack";

    public override void StartAttack()
    {
        if (!playerInRange)
            return;

        if (attackTimer > 0f)
            return;

        if (animator.GetInteger("AttackIndex") != 0)
            return;

        // Trigger heavy attack
        animator.SetInteger("AttackIndex", 1);
        animator.SetBool("CanAttack", true);

        agent.isStopped = true;

        Debug.Log($"[{nameof(EnemyHeavy)}] '{agent.name}' heavy attack triggered");

        attackTimer = attackCooldown;
    }

    protected override void HandleAttackLogic()
    {
        if (!playerInRange) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool inAttackState = IsInAttackState(stateInfo);

        // If currently playing attack animation, do nothing
        if (inAttackState)
            return;

        // Reduce cooldown timer
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                attackTimer = 0f;

                if (playerInRange)
                {
                    // Ready for next attack cycle
                    animator.SetBool("CanAttack", true);
                    animator.SetInteger("AttackIndex", 0);
                    agent.isStopped = false;
                }
            }

            return;
        }

        // Reset attack animation after finishing
        if (!inAttackState && animator.GetInteger("AttackIndex") != 0)
        {
            animator.SetInteger("AttackIndex", 0);
            animator.SetBool("CanAttack", true);
            agent.isStopped = false;
        }
    }

    protected override bool IsInAttackState(AnimatorStateInfo stateInfo)
    {
        return stateInfo.IsName(AttackStateName);
    }

    public override void SetPlayerInRange(bool inRange)
    {
        base.SetPlayerInRange(inRange);

        if (inRange && attackTimer <= 0f)
        {
            // Player in range and attack ready → CanAttack = true, AttackIndex = 0
            animator.SetBool("CanAttack", true);
            animator.SetInteger("AttackIndex", 0);
        }
        else if (!inRange)
        {
            // Player out of range → stop attack
            animator.SetBool("CanAttack", false);
            animator.SetInteger("AttackIndex", 0);
            agent.isStopped = false;
        }
    }
}