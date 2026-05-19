using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll_debug : MonoBehaviour
{
    Rigidbody[] rigidBodies;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public void EnableRagdoll()
    {
        foreach (var rb in rigidBodies)
        {
            rb.isKinematic = false;
        }
        anim.enabled = false;
    }

    public void DisableRagdoll()
    {
        foreach (var rb in rigidBodies)
        {
            rb.isKinematic = true;
        }
        anim.enabled = true;
    }
}
