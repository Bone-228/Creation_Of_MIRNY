using UnityEngine;

public class tutorialGateManipulation : MonoBehaviour
{
    public Animator animator;                 // Reference to the gate Animator
    public test_interaction interaction;      // Reference to the interaction script

    void Start()
    {
        // Optional safety checks
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (interaction == null) return;

        if (interaction.success)
        {
            animator.SetBool("Open", true);
            animator.SetBool("Closed", false);
        }
        else
        {
            animator.SetBool("Open", false);
            animator.SetBool("Closed", true);
        }
    }
}