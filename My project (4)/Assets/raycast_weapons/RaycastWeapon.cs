using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public bool isFiring = false;
    public void StartFiring() 
    { 
        isFiring = true;
    }
    public void StopFiring() 
    {
        isFiring = false;
    }   
}
