using UnityEngine;
using Assets.scripts_camera;
using UnityEngine.Animations.Rigging;

public class BattleZoneHandler : MonoBehaviour
{
    [Header("Zone Settings")]
    public float zoneRadius = 10f;

    [Header("References")]
    public Transform player;
    public ThirdPersonCam playerCam;
    public RigBuilder rigBuilder;

    public bool playerInside = false;

    void Start()
    {
        // Auto-find references if not assigned
        if (player != null && rigBuilder == null)
            rigBuilder = player.GetComponent<RigBuilder>();
    }

    void Update()
    {
        if (player == null || playerCam == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= zoneRadius)
        {
            if (!playerInside)
            {
                playerInside = true;

                // Enable combat camera
                playerCam.combatLocked = true;
                playerCam.camStyle = ThirdPersonCam.CameraStyle.Combat;

                // Enable RigBuilder
                if (rigBuilder != null)
                    rigBuilder.enabled = true;
            }
        }
        else
        {
            if (playerInside)
            {
                playerInside = false;

                // Return camera to normal
                playerCam.combatLocked = false;
                playerCam.camStyle = ThirdPersonCam.CameraStyle.Basic;

                // Disable RigBuilder
                if (rigBuilder != null)
                    rigBuilder.enabled = false;
            }
        }
        Debug.Log("CAMERA MODE IS - " + playerCam.camStyle);
    }

    // Draw zone in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, zoneRadius);
    }
}