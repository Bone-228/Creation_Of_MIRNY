using System.Collections;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [Header("References")]
    public Transform platform;       // Platform transform

    [Header("Settings")]
    public float moveDistance = 5f;  // How much to move up/down
    public float moveSpeed = 1f;     // How fast the platform moves
    public float waitTime = 3f;      // Wait at top and bottom

    private Vector3 startPosition;
    private Vector3 topPosition;
    private Vector3 bottomPosition;

    private void Start()
    {
        if (platform == null)
            platform = transform;

        startPosition = platform.position;
        bottomPosition = startPosition;
        topPosition = startPosition + Vector3.up * moveDistance;

        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Wait at bottom
            yield return new WaitForSeconds(waitTime);

            // Move up
            yield return MoveToPosition(topPosition);

            // Wait at top
            yield return new WaitForSeconds(waitTime);

            // Move down
            yield return MoveToPosition(bottomPosition);
        }
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(platform.position, target) > 0.01f)
        {
            platform.position = Vector3.MoveTowards(platform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
} 