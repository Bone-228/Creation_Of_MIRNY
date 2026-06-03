using UnityEngine;

public class gateControll : MonoBehaviour
{
    [Header("References")]
    public Animator gateAnimator;

    [Tooltip("Invisible wall/collider blocking the gate")]
    public GameObject gateBlocker;

    public GateAudio gateAudio;
    public void OpenGate()
    {
        gateAudio.PlayOpenSound();
        // Open animation
        if (gateAnimator != null)
        {
            gateAnimator.SetInteger("State", 1);
        }

        // Disable invisible wall
        if (gateBlocker != null)
        {
            gateBlocker.SetActive(false);
        }
    }

    public void CloseGate()
    {
        // Close animation
        if (gateAnimator != null)
        {
            gateAnimator.SetInteger("State", 0);
        }

        // Enable invisible wall
        if (gateBlocker != null)
        {
            gateBlocker.SetActive(true);
        }
    }
}