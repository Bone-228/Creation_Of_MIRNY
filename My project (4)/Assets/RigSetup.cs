using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class RigSetup : MonoBehaviour
{
    [SerializeField]
    [Header("Rig References")]
    public Rig rifleHandRig;
    public Rig pistolHandRig;
    [Header("Shooter Controller References")]
    public ShooterController sController;

    void Update()
    {
        if (sController.CurrentWeapon is Pistol)
        {
            pistolHandRig.weight = 1f;
            rifleHandRig.weight = 0f;
        }
        else if (sController.CurrentWeapon is SMG)
        {
            pistolHandRig.weight = 0f;
            rifleHandRig.weight = 1f;
        }
    }
}
