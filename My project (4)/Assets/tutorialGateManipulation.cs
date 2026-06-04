using UnityEngine;

public class tutorialGateManipulation : MonoBehaviour
{
    public Animator animator;                 // Reference to the gate Animator
    public test_interaction interaction;      // Reference to the interaction script
    public GateAudio gateAudio;
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
            animator.SetInteger("State", 1);
            gateAudio.PlayOpenSound();
            Debug.Log("Gate opened!");
        }
        else
        {
            animator.SetInteger("State", 0);
        }
    }
}