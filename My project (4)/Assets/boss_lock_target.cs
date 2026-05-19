using UnityEngine;

public class boss_lock_target : MonoBehaviour
{
    public Transform player;
    public Transform bone;

    void Update()
    {
        if (player == null || bone == null)
            return;

        // Direction from bone to player
        Vector3 direction = player.position - bone.position;

        // Ignore height difference so it rotates only horizontally
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Apply only Y rotation
            bone.rotation = Quaternion.Euler(
                bone.rotation.eulerAngles.x,
                targetRotation.eulerAngles.y,
                bone.rotation.eulerAngles.z
            );
        }
    }
}