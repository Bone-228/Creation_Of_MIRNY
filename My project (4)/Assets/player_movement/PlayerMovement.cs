using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Aiming")]
    public bool isAiming { get; private set; }
    private bool externalAimingOverride = false;

    [Header("Base Movement Speeds")]
    public float baseSlowWalkSpeed = 2f;
    public float baseWalkSpeed = 4f;
    public float baseSprintSpeed = 7f;
    public float baseWallRunSpeed = 8f;

    [Header("Runtime Movement Speeds")]
    public float slowWalkSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float wallRunSpeed;

    private float normalMoveSpeed;

    public Transform orientation;

    [Header("Air Control")]
    public float airAcceleration = 4f;
    public float airDrag = 1f;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 45f;

    [Range(0.01f, 0.5f)]
    public float slopeSmoothness = 0.15f;

    bool exitingSlope;

    private RaycastHit slopeHit;

    float slopeLerp;

    [Header("Other settings")]
    public float groundDrag = 6f;

    public float jumpForce = 5f;

    public float jumpCooldown = 0.25f;

    bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight = 2f;

    public LayerMask whatIsGround;

    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 movementDirection;

    Rigidbody rb;

    public enum MovementState
    {
        slowWalk,
        walk,
        sprint,
        wallrunning,
        air
    }

    public MovementState state;

    public bool wallrunning;

    public bool isTeleporting { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        readyToJump = true;

        if (orientation == null)
            orientation = transform;

        // Apply modifiers on start
        RecalculateMovement();
    }

    // ───────────────────────── MODIFIER RECALCULATION ─────────────────────────

    public void RecalculateMovement()
    {
        // Reset runtime speeds to base values
        slowWalkSpeed = baseSlowWalkSpeed;

        walkSpeed = baseWalkSpeed;

        sprintSpeed = baseSprintSpeed;

        wallRunSpeed = baseWallRunSpeed;

        // Apply equipped modifiers
        foreach (ModifierData modifier in GameManager.Instance.equippedModifiers)
        {
            sprinterModifier speedModifier =
                modifier as sprinterModifier;

            if (speedModifier != null)
            {
                slowWalkSpeed += speedModifier.speedBonus;

                walkSpeed += speedModifier.speedBonus;

                sprintSpeed += speedModifier.speedBonus;

                wallRunSpeed += speedModifier.speedBonus;
            }
        }

        Debug.Log("Movement recalculated!");
    }

    // ───────────────────────── UPDATE ─────────────────────────

    private void Update()
    {
        if (isTeleporting) return;

        Vector3 origin = transform.position + Vector3.up * 0.1f;

        grounded = Physics.SphereCast(
            origin,
            0.35f,
            Vector3.down,
            out slopeHit,
            (playerHeight * 0.5f) + 0.2f,
            whatIsGround
        );

        myInput();

        StateHandler();

        isAiming = externalAimingOverride || Input.GetMouseButton(1);

        rb.linearDamping = grounded ? groundDrag : airDrag;

        float target = OnSlope() ? 1f : 0f;

        slopeLerp = Mathf.Lerp(
            slopeLerp,
            target,
            Time.deltaTime / slopeSmoothness
        );
    }

    private void FixedUpdate()
    {
        if (isTeleporting) return;

        movePlayer();

        LimitSpeed();
    }

    // ───────────────────────── INPUT ─────────────────────────

    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    // ───────────────────────── STATE HANDLER ─────────────────────────

    private void StateHandler()
    {
        if (grounded && Input.GetKey(KeyCode.LeftShift))
        {
            state = MovementState.sprint;

            normalMoveSpeed = sprintSpeed;
        }
        else if (grounded && Input.GetKey(KeyCode.LeftAlt))
        {
            state = MovementState.slowWalk;

            normalMoveSpeed = slowWalkSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walk;

            normalMoveSpeed = walkSpeed;
        }
        else if (wallrunning)
        {
            state = MovementState.wallrunning;

            normalMoveSpeed = wallRunSpeed;
        }
        else
        {
            state = MovementState.air;

            // Prevent stale speed bug
            normalMoveSpeed = walkSpeed;
        }
    }

    // ───────────────────────── MOVEMENT ─────────────────────────

    private void movePlayer()
    {
        movementDirection =
            orientation.forward * verticalInput +
            orientation.right * horizontalInput;

        Vector3 dir = movementDirection.normalized;

        Vector3 slopeDir = GetSlopeMoveDirection();

        Vector3 finalDir = Vector3.Lerp(dir, slopeDir, slopeLerp);

        if (grounded)
        {
            rb.AddForce(
                finalDir * normalMoveSpeed * 10f,
                ForceMode.Force
            );
        }
        else
        {
            Vector3 airVel = rb.linearVelocity;

            Vector3 targetVel = dir * normalMoveSpeed;

            Vector3 velocityChange =
                targetVel -
                new Vector3(airVel.x, 0f, airVel.z);

            rb.AddForce(
                velocityChange * airAcceleration,
                ForceMode.Acceleration
            );
        }

        rb.useGravity = !OnSlope();
    }

    // ───────────────────────── SPEED LIMIT ─────────────────────────

    private void LimitSpeed()
    {
        Vector3 flatVel =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        if (flatVel.magnitude > normalMoveSpeed)
        {
            Vector3 limitedVel =
                flatVel.normalized * normalMoveSpeed;

            rb.linearVelocity =
                new Vector3(
                    limitedVel.x,
                    rb.linearVelocity.y,
                    limitedVel.z
                );
        }
    }

    // ───────────────────────── JUMP ─────────────────────────

    private void Jump()
    {
        exitingSlope = true;

        rb.linearVelocity =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    // ───────────────────────── SLOPES ─────────────────────────

    private bool OnSlope()
    {
        if (!grounded) return false;

        float angle =
            Vector3.Angle(Vector3.up, slopeHit.normal);

        return angle > 0f && angle < maxSlopeAngle;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(
            movementDirection,
            slopeHit.normal
        ).normalized;
    }

    // ───────────────────────── TELEPORT ─────────────────────────

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        Physics.SyncTransforms();

        rb.isKinematic = true;

        transform.SetPositionAndRotation(position, rotation);

        rb.position = position;

        rb.rotation = rotation;

        rb.linearVelocity = Vector3.zero;

        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = false;
    }

    public IEnumerator TeleportLock()
    {
        isTeleporting = true;

        slopeLerp = 0f;

        exitingSlope = true;

        grounded = false;

        yield return null;

        yield return null;

        isTeleporting = false;
    }
}