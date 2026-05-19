using UnityEngine;

public class Enemy_Range : Enemy
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementAnimation();

        if (playerInRange)
        {
            animator.SetBool("isAiming", true);
        }
        else 
        { 
            animator.SetBool("isAiming", false);
        }
    }
}
