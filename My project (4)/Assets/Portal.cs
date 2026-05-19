using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform[] destination;
    public int random;

    private void OnTriggerEnter(Collider other)
    {
        if (destination == null || destination.Length == 0) return;

        random = Random.Range(0, destination.Length);

        var root = other.transform.root;

        if (!root.CompareTag("Player") && !other.CompareTag("Player"))
            return;

        var pm = other.GetComponentInParent<PlayerMovement>();

        if (pm != null)
        {
            StartCoroutine(TeleportSequence(pm));
        }
    }

    private IEnumerator TeleportSequence(PlayerMovement pm)
    {
        Vector3 pos = destination[random].position;

        Quaternion rot = Quaternion.Euler(
            0f,
            destination[random].eulerAngles.y,
            0f
        );

        // 1. teleport instantly
        pm.Teleport(pos, rot);

        // 2. freeze movement + slope + grounding
        yield return StartCoroutine(pm.TeleportLock());

        // 3. sync animator BEFORE rig update
        var animator = pm.GetComponentInChildren<Animator>();
        if (animator != null)
            animator.Update(0f);

        // 4. rebuild rig AFTER stabilization
        var rig = pm.GetComponentInChildren<UnityEngine.Animations.Rigging.RigBuilder>();

        if (rig != null)
        {
            rig.enabled = false;
            yield return null;
            rig.enabled = true;
            rig.Build();
        }

        yield return null;
    }
}