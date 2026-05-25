using UnityEngine;

public class gateControll : MonoBehaviour
{
    public Animator gateAnimator;

    public void OpenGate()
    {
        gateAnimator.SetInteger("State", 1);
    }

    public void CloseGate()
    {
        gateAnimator.SetInteger("State", 0);
        Debug.Log($"Gate {gameObject.name} closed");
    }
}