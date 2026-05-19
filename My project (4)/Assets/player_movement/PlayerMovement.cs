using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Aiming")]
    public bool isAiming { get; private set; }
    private bool externalAimingOverride = false;

    [Header("Movement")]
    private float normalMoveSpeed;
    public float slowWalkSpeed = 2f;
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float wallRunSpeed = 8f;
    public Transform orientation;

    [Header("Air Control")]
    public float airAcceleration = 4f;   // 🔥 NEW
    public float airDrag = 1f;           // 🔥 NEW

    [Header("Slope Handling")]
    public float maxSlopeAngle = 45f;
    [Range(0.01f, 0.5f)] public float slopeSmoothness = 0.15f;
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

    public enum MovementState { slowWalk, walk, sprint, wallrunning, air }
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
    }

    private void Update()
    {
        if (isTeleporting) return;

        Vector3 origin = transform.position + Vector3.up * 0.1f;
        grounded = Physics.SphereCast(origin, 0.35f, Vector3.down, out slopeHit,
            (playerHeight * 0.5f) + 0.2f, whatIsGround);

        myInput();
        StateHandler();

        isAiming = externalAimingOverride || Input.GetMouseButton(1);

        // 🔥 Improved damping
        rb.linearDamping = grounded ? groundDrag : airDrag;

        float target = OnSlope() ? 1f : 0f;
        slopeLerp = Mathf.Lerp(slopeLerp, target, Time.deltaTime / slopeSmoothness);
    }

    private void FixedUpdate()
    {
        if (isTeleporting) return;

        movePlayer();
        LimitSpeed(); // 🔥 NEW
    }

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
        }
    }

    private void movePlayer()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 dir = movementDirection.normalized;

        Vector3 slopeDir = GetSlopeMoveDirection();
        Vector3 finalDir = Vector3.Lerp(dir, slopeDir, slopeLerp);

        if (grounded)
        {
            rb.AddForce(finalDir * normalMoveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            // 🔥 Better air control (no infinite acceleration)
            Vector3 airVel = rb.linearVelocity;
            Vector3 targetVel = dir * normalMoveSpeed;

            Vector3 velocityChange = targetVel - new Vector3(airVel.x, 0f, airVel.z);
            rb.AddForce(velocityChange * airAcceleration, ForceMode.Acceleration);
        }

        rb.useGravity = !OnSlope();
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > normalMoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * normalMoveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (!grounded) return false;
        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
        return angle > 0f && angle < maxSlopeAngle;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;
    }

    // ───────────────────────── TELEPORT ─────────────────────────

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

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