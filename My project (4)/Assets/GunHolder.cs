using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    public Transform gunHolder;
    public Transform playerBody;
    public Transform cameraTransform;

    void Update()
    {
        // Match gunHolder's local rotation to camera's local rotation (up/down movement)
        Vector3 gunEuler = gunHolder.localEulerAngles;
        gunEuler.x = cameraTransform.localEulerAngles.x;
        gunHolder.localEulerAngles = gunEuler;
    }

}
