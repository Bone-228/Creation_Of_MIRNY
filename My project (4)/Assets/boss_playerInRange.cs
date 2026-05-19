using UnityEngine;

public class boss_playerInRange : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRadius = 10f;

    [Header("References")]
    public Transform player;
    public boss_lock_target lockTargetScript;

    void Start()
    {
        // Auto-find player by tag if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null || lockTargetScript == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Activate / deactivate target lock
        lockTargetScript.enabled = distance <= detectionRadius;
    }

    // Draw sphere in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}