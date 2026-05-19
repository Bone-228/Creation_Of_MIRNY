using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast_Weapon : MonoBehaviour
{
    [SerializeField]
    public Transform shootPoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("FIRE FIRE FIRE");
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit)) 
        {
            Debug.Log("We hit " + hit.collider.name);
        }

       
       
    }
}
