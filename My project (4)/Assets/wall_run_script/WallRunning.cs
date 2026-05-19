using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;
    public float wallJumpUpForce;
    public float wallJumpSideForce;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    private Rigidbody rb;
    private PlayerMovement pm;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMashine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
        {
            WallRunningMovement();
        }
    }

    // Detect walls
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(
            transform.position,
            orientation.right,
            out rightWallHit,
            wallCheckDistance,
            whatIsWall
        );

        wallLeft = Physics.Raycast(
            transform.position,
            -orientation.right,
            out leftWallHit,
            wallCheckDistance,
            whatIsWall
        );
    }

    // Detect if above ground
    private bool AboveGround()
    {
        return !Physics.Raycast(
            transform.position,
            Vector3.down,
            minJumpHeight,
            whatIsGround
        );
    }

    // State machine
    private void StateMashine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) &&
            verticalInput > 0 &&
            AboveGround() &&
            !exitingWall)
        {
            if (!pm.wallrunning)
            {
                StartWallRun();
            }

            if (Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }
        }
        // State 2 - Exiting
        else if (exitingWall)
        {
            if (pm.wallrunning)
            {
                StopWallRun();
            }

            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if (exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }
        else
        {
            if (pm.wallrunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        rb.useGravity = false;

    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;

        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        Vector3 wallNormal = wallRight
            ? rightWallHit.normal
            : leftWallHit.normal;

        // Calculate wall forward direction
        Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up);

        // Choose correct direction
        if ((orientation.forward - wallForward).magnitude >
            (orientation.forward + wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        // Move along wall
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // Stick to wall
        if (!(wallLeft && horizontalInput > 0) &&
            !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);
        }
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        rb.useGravity = true;

    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        StopWallRun();

        Vector3 wallNormal = wallRight
            ? rightWallHit.normal
            : leftWallHit.normal;

        // Always jump UP + away from wall
        Vector3 jumpForce =
            Vector3.up * wallJumpUpForce +
            wallNormal * wallJumpSideForce;

        // Remove downward momentum only
        Vector3 velocity = rb.linearVelocity;

        if (velocity.y < 0f)
        {
            velocity.y = 0f;
        }

        rb.linearVelocity = velocity;

        rb.AddForce(jumpForce, ForceMode.Impulse);
    }
}