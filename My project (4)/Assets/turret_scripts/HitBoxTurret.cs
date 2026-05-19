using UnityEngine;

public class HitBoxTurret : MonoBehaviour
{
    public turret_health turretHealth;

    private void Awake()
    {
        if (turretHealth == null)
        {
            turretHealth =
                GetComponentInParent<turret_health>();
        }
    }

    public void HitByRaycast()
    {
        if (turretHealth != null)
        {
            turretHealth.DestroyTurret();
        }
    }
}