using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMashine stateMachine;
    public AiStateID initialState;
    public NavMeshAgent navMeshAgent;

    public Ragdoll_debug ragdollDebug;
    public UIHealthbar healthBar;

    public Transform player;

    public float sightSpotDistance = 20f;

    public int miriumValue = 10; // ✅ changed to int (better for currency)

    public int scrapsToGive = 10;

    public int healthToSteal = 10;

    public float attackRange = 2.5f;

    private bool _isSeeingPlayer;

    private const float EyeHeight = 1.6f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new AiStateMashine(this);
        stateMachine.RegisterState(new AiChasePlayer());
        stateMachine.RegisterState(new AiDeath());
        stateMachine.RegisterState(new AiIdle());
        stateMachine.RegisterState(new AiAttack());
        stateMachine.ChangeState(initialState);

        ragdollDebug = GetComponent<Ragdoll_debug>();
        healthBar = GetComponentInChildren<UIHealthbar>();

        navMeshAgent.stoppingDistance = attackRange;
        navMeshAgent.autoBraking = true;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        stateMachine.Update();

        bool sees = CanSeePlayer();

        if (sees && !_isSeeingPlayer)
            Debug.Log($"{name} sees player");
        else if (!sees && _isSeeingPlayer)
            Debug.Log($"{name} lost sight of player");

        _isSeeingPlayer = sees;
    }

    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector3 origin = transform.position + Vector3.up * EyeHeight;
        Vector3 direction = player.position - origin;
        float distance = direction.magnitude;

        if (distance > sightSpotDistance)
            return false;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, sightSpotDistance))
        {
            if (hit.transform == player || hit.transform.root == player.root)
                return true;
        }

        return false;
    }
}