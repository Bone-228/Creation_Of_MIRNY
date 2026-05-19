using UnityEngine;
using Assets.scripts_camera;
using UnityEngine.Animations.Rigging;

public class specialBattleZone : BattleZoneHandler
{
    [Header("Hand Rigs")]
    public Rig rightHandRig;
    public Rig leftHandRig;

    void Update()
    {
        if (player == null || playerCam == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= zoneRadius)
        {
            if (!playerInside)
            {
                playerInside = true;

                // Combat camera
                playerCam.combatLocked = true;
                playerCam.camStyle = ThirdPersonCam.CameraStyle.Combat;

                // Disable hand rigs
                if (rightHandRig != null)
                    rightHandRig.weight = 0f;

                if (leftHandRig != null)
                    leftHandRig.weight = 0f;
            }
        }
        else
        {
            if (playerInside)
            {
                playerInside = false;

                // Return camera
                playerCam.combatLocked = false;
                playerCam.camStyle = ThirdPersonCam.CameraStyle.Basic;

                // Enable hand rigs again
                if (rightHandRig != null)
                    rightHandRig.weight = 1f;

                if (leftHandRig != null)
                    leftHandRig.weight = 1f;
            }
        }
    }
}