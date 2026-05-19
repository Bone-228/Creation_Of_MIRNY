using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    public Animator animator;

    float speedSmooth = 0f;
    const float speedSmoothTime = 0.2f;

    [SerializeField]
    public float maxMovementSpeed = 8f;

    [Header("Attack Settings")]
    [SerializeField]
    protected float attackCooldown = 2f; // now editable in Inspector

    protected float attackTimer = 0f;

    public bool playerInRange;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        HandleMovementAnimation();
        HandleAttackLogic();
    }

    public void HandleMovementAnimation()
    {
        float rawSpeed = agent.velocity.magnitude;
        float normalizedSpeed = Mathf.Clamp01(rawSpeed / maxMovementSpeed);

        speedSmooth = Mathf.Lerp(speedSmooth, normalizedSpeed, Time.deltaTime / speedSmoothTime);
        animator.SetFloat("Speed", speedSmooth);
    }

    public virtual void SetPlayerInRange(bool inRange)
    {
        playerInRange = inRange;

        if (inRange)
        {
            animator.SetBool("CanAttack", true);
            animator.SetInteger("AttackIndex", 0);
        }
        else
        {
            animator.SetBool("CanAttack", false);
            animator.SetInteger("AttackIndex", 0);
            agent.isStopped = false;
        }

        Debug.Log($"[{nameof(Enemy)}] SetPlayerInRange({inRange}) called for '{agent.name}'");
    }

    // Called externally by AiAttack state
    public virtual void StartAttack()
    {
        if (!playerInRange)
            return;

        if (attackTimer > 0f)
            return;

        if (animator.GetInteger("AttackIndex") != 0)
            return;

        int attackIndex = Random.Range(1, 3);

        animator.SetInteger("AttackIndex", attackIndex);
        animator.SetBool("CanAttack", true);

        agent.isStopped = true;

        Debug.Log($"[{nameof(Enemy)}] '{agent.name}' attack triggered externally (Attack_{attackIndex})");

        attackTimer = attackCooldown;
    }

    protected virtual void HandleAttackLogic()
    {
        if (!playerInRange) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool inAttackState = IsInAttackState(stateInfo);

        if (inAttackState)
            return;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                attackTimer = 0f;

                if (playerInRange)
                    animator.SetBool("CanAttack", true);
            }

            return;
        }

        if (!inAttackState && animator.GetInteger("AttackIndex") != 0)
        {
            animator.SetInteger("AttackIndex", 0);
            animator.SetBool("CanAttack", false);
            agent.isStopped = false;
        }
    }

    protected virtual bool IsInAttackState(AnimatorStateInfo stateInfo)
    {
        return stateInfo.IsName("Attack_1") || stateInfo.IsName("Attack_2");
    }

    // Public setter to change cooldown at runtime
    public void SetAttackCooldown(float newCooldown)
    {
        attackCooldown = Mathf.Max(0.1f, newCooldown); // prevents zero or negative
    }
}