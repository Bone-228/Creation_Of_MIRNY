using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigLayerAimManager : MonoBehaviour
{
    public detectAiming aimDec;

    public Rig weaponPoseRig;
    public Rig weaponAimRig;

    public float transitionSpeed = 5f;

    void Awake()
    {
        var player = GameObject.FindWithTag("Player");

        if (player != null)
            aimDec = player.GetComponent<detectAiming>();

        if (aimDec == null)
            aimDec = FindObjectOfType<detectAiming>();
    }

    void LateUpdate()
    {
        if (aimDec == null) return;

        float targetAim = aimDec.isAiming ? 1f : 0f;
        float targetPose = aimDec.isAiming ? 0f : 1f;

        weaponAimRig.weight = Mathf.MoveTowards(
            weaponAimRig.weight,
            targetAim,
            transitionSpeed * Time.deltaTime
        );

        weaponPoseRig.weight = Mathf.MoveTowards(
            weaponPoseRig.weight,
            targetPose,
            transitionSpeed * Time.deltaTime
        );
    }
}