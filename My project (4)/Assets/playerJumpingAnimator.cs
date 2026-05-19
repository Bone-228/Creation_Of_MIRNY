using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class playerJumpingAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Jump Layer Settings")]
    [SerializeField] private string jumpLayerName = "Jumping";
    [SerializeField] private string inAirParamName = "inAir";
    [SerializeField][Range(0f, 0.5f)] private float weightSmoothTime = 0.08f;

    private int jumpLayerIndex = -1;
    private int inAirHash;

    private float weightVelocity;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        if (animator == null || playerMovement == null)
        {
            Debug.LogWarning("playerJumpingAnimator: Missing references.");
            enabled = false;
            return;
        }

        jumpLayerIndex = animator.GetLayerIndex(jumpLayerName);

        if (jumpLayerIndex < 0)
            Debug.LogWarning($"Jump layer '{jumpLayerName}' not found in Animator.");

        inAirHash = Animator.StringToHash(inAirParamName);
    }

    void Update()
    {
        if (jumpLayerIndex < 0)
            return;

        bool isInAir = playerMovement.state == PlayerMovement.MovementState.air;

        // Set animator bool parameter
        animator.SetBool(inAirHash, isInAir);

        // Smooth layer blending
        float targetWeight = isInAir ? 1f : 0f;
        float currentWeight = animator.GetLayerWeight(jumpLayerIndex);

        float newWeight = Mathf.SmoothDamp(
            currentWeight,
            targetWeight,
            ref weightVelocity,
            weightSmoothTime
        );

        // Snap to exact value when very close
        if (Mathf.Abs(newWeight - targetWeight) < 0.001f)
            newWeight = targetWeight;

        animator.SetLayerWeight(jumpLayerIndex, newWeight);
    }
}