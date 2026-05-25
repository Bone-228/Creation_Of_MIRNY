using UnityEngine;

public class gateControll : MonoBehaviour
{
    public Animator gateAnimator;

    public int currentState = 0; // local tracking (optional but useful)

    public void OpenGate()
    {
        if (gateAnimator == null) return;

        // already open → do nothing
        if (currentState == 1)
            return;

        currentState = 1;
        gateAnimator.SetInteger("State", 1);
    }

    public void CloseGate()
    {
        if (gateAnimator == null) return;

        // already closed → do nothing
        if (currentState == 0)
            return;

        currentState = 0;
        gateAnimator.SetInteger("State", 0);
    }
}