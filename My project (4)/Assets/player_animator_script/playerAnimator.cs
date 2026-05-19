using UnityEngine;

public class playerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private string playerSpeedParameter = "playerSpeed";

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float speedDampTime = 0.1f;
    [SerializeField] private bool snapToSteps = false;

    private Vector3 lastPosition;
    private int speedHash;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (player != null && playerRigidbody == null)
            playerRigidbody = player.GetComponent<Rigidbody>();

        lastPosition = (player != null) ? player.transform.position : transform.position;

        speedHash = Animator.StringToHash(playerSpeedParameter);
    }

    void Update()
    {
        Vector3 movementVec = Vector3.zero;
        float rawSpeed = 0f;

        if (playerRigidbody != null)
        {
            movementVec = playerRigidbody.linearVelocity;
            rawSpeed = movementVec.magnitude;
            lastPosition = playerRigidbody.transform.position;
        }
        else if (player != null)
        {
            Vector3 current = player.transform.position;
            Vector3 delta = current - lastPosition;
            float dt = Mathf.Max(Time.deltaTime, 1e-6f);

            movementVec = delta / dt;
            rawSpeed = delta.magnitude / dt;

            lastPosition = current;
        }

        float normalized = Mathf.InverseLerp(0f, runSpeed, rawSpeed);
        normalized = Mathf.Clamp01(normalized);

        if (snapToSteps)
        {
            float[] steps = { 0f, 0.5f, 1f };
            float best = steps[0];
            float bestDist = Mathf.Abs(normalized - best);

            for (int i = 1; i < steps.Length; i++)
            {
                float d = Mathf.Abs(normalized - steps[i]);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = steps[i];
                }
            }

            normalized = best;
        }

        animator.SetFloat(speedHash, normalized, speedDampTime, Time.deltaTime);
    }
}