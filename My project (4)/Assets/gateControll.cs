using UnityEngine;

public class gateControll : MonoBehaviour
{
    public Animator gateAnimator;
    public GameObject invisibleWall;

    void Update()
    {
        if (gateAnimator != null && invisibleWall != null)
        {
            if (gateAnimator.GetBool("Open"))
            {
                invisibleWall.SetActive(false);
            }
            else
            {
                invisibleWall.SetActive(true);
            }
        }
    }
}