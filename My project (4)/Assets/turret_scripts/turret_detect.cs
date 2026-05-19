using UnityEngine;

/*
Pseudocode / Plan (detailed):
- Purpose: When the player enters the turret's detection range, notify / enable the turret_shoot
  component so the turret begins shooting. When the player leaves the range, disable/notify stop.

- Setup in Start():
  - Cache the player Transform by finding GameObject tagged "Player".
  - Cache the original turret target from `turretIK.target` (if set).
  - Create a hidden `smoothTarget` GameObject as in the original design and assign it to `turretIK.target`.
  - Attempt to locate a `turret_shoot` component automatically if the `turretShoot` field is not
    assigned in the inspector. Search:
      1) on this GameObject via `GetComponent<turret_shoot>()`
      2) on the `turretIK` GameObject
      3) in children via `GetComponentInChildren<turret_shoot>()`

- Runtime in Update():
  - If `smoothTarget` or `turretIK` are null, do nothing.
  - Compute `desiredPosition` exactly as before (player if within range, otherwise fallback).
  - Smoothly move `smoothTarget.position` toward `desiredPosition`.
  - Ensure `turretIK.target` points to `smoothTarget` each frame.
  - Determine `playerInRange` boolean: true when player exists and distance <= range.
  - Track previous state with `playerPreviouslyInRange`.
    - On transition false -> true (enter range):
      - If `turretShoot` was found: enable it (`turretShoot.enabled = true`) so it can run,
        and send a non-fatal message `OnPlayerSpotted` on its GameObject using `SendMessage`
        (this allows compatibility with various implementations).
    - On transition true -> false (exit range):
      - If `turretShoot` was found: disable it and send `OnPlayerLost` message (DontRequireReceiver).
  - This avoids hard dependency on a particular method name while still notifying the shooter.

- Cleanup:
  - Destroy the created `smoothTarget` in OnDestroy.

Notes:
- Uses SendMessage with DontRequireReceiver so missing handler methods won't throw errors.
- The approach keeps the smooth-target logic intact and only adds entry/exit notifications.
*/

public class turret_detect : MonoBehaviour
{
    [SerializeField]
    public float range = 10f;

    [SerializeField]
    public float smoothSpeed = 5f; // larger = faster smoothing

    public TurretIK turretIK;

    // Optional reference to the shooting component; if not set, Start() will try to find one.
    public turret_shoot turretShoot;

    private Transform playerTransform;
    private Transform smoothTarget;
    private Transform originalTarget;

    // Tracks previous detection state so we only notify on transitions.
    private bool playerPreviouslyInRange = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, range);
    }

    private void Start()
    {
        // Cache player
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        // Cache original turret target if present
        if (turretIK != null)
        {
            originalTarget = turretIK.target;
        }

        // Create a smooth target GameObject and assign it to turretIK
        var go = new GameObject("turret_detect_smooth_target");
        go.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
        go.transform.parent = transform;
        // Initialize position: prefer original target, else some point in front of turret
        Vector3 initPos = (originalTarget != null) ? originalTarget.position : (transform.position + transform.forward * 10f);
        go.transform.position = initPos;
        smoothTarget = go.transform;

        if (turretIK != null)
        {
            turretIK.target = smoothTarget;
        }

        // Try to locate a turret_shoot component if none was assigned in inspector.
        if (turretShoot == null)
        {
            turretShoot = GetComponent<turret_shoot>();
        }

        if (turretShoot == null && turretIK != null)
        {
            turretShoot = turretIK.GetComponent<turret_shoot>();
        }

        if (turretShoot == null)
        {
            turretShoot = GetComponentInChildren<turret_shoot>();
        }

        // Ensure initial enabled state reflects whether player is currently in range
        bool initiallyInRange = false;
        if (playerTransform != null)
        {
            initiallyInRange = Vector3.Distance(transform.position, playerTransform.position) <= range;
        }

        playerPreviouslyInRange = initiallyInRange;
        if (turretShoot != null)
        {
            turretShoot.enabled = initiallyInRange;
            if (initiallyInRange)
            {
                turretShoot.gameObject.SendMessage("OnPlayerSpotted", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void Update()
    {
        if (smoothTarget == null || turretIK == null)
        {
            return;
        }

        Vector3 desiredPosition;
        bool currentInRange = false;

        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= range)
            {
                // Spotted: move toward the player's position
                desiredPosition = playerTransform.position;
                currentInRange = true;
            }
            else
            {
                // Not in range: fall back to original target or default forward point
                desiredPosition = (originalTarget != null) ? originalTarget.position : (transform.position + transform.forward * 10f);
            }
        }
        else
        {
            // No player found: fallback behavior
            desiredPosition = (originalTarget != null) ? originalTarget.position : (transform.position + transform.forward * 10f);
        }

        // Exponential smoothing factor (frame-rate independent).
        // Larger smoothSpeed -> faster convergence.
        float t = 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime);
        smoothTarget.position = Vector3.Lerp(smoothTarget.position, desiredPosition, t);

        // Ensure turret points at the smoothing target
        turretIK.target = smoothTarget;

        // Detect transitions and notify / enable shooter accordingly
        if (currentInRange && !playerPreviouslyInRange)
        {
            // Entered range
            if (turretShoot != null)
            {
                turretShoot.enabled = true;
                turretShoot.gameObject.SendMessage("OnPlayerSpotted", SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (!currentInRange && playerPreviouslyInRange)
        {
            // Exited range
            if (turretShoot != null)
            {
                turretShoot.gameObject.SendMessage("OnPlayerLost", SendMessageOptions.DontRequireReceiver);
                turretShoot.enabled = false;
            }
        }

        playerPreviouslyInRange = currentInRange;
    }

    private void OnDestroy()
    {
        if (smoothTarget != null)
        {
            DestroyImmediate(smoothTarget.gameObject);
        }
    }
}
