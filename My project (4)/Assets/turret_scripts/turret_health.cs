
using UnityEngine;

public class turret_health : MonoBehaviour
{
    [Header("References")]
    public turret_shoot turretShoot;
    public TurretIK turretIK;
    public turret_detect turretDetect;

    [Header("Death")]
    public float destroyDelay = 2f;

    bool isDead = false;

    private void Awake()
    {
        if (turretShoot == null)
        {
            turretShoot = GetComponent<turret_shoot>();
        }

        if (turretIK == null)
        {
            turretIK = GetComponent<TurretIK>();
        }

        if (turretDetect == null)
        {
            turretDetect = GetComponent<turret_detect>();
        }
    }

    public void DestroyTurret()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Turret destroyed");

        // Stop shooting + play death animation
        if (turretShoot != null)
        {
            turretShoot.KillTurret();
            turretShoot.enabled = false;
        }

        // Stop aiming IK
        if (turretIK != null)
        {
            turretIK.enabled = false;
        }

        // Stop detection logic
        if (turretDetect != null)
        {
            turretDetect.enabled = false;
        }
    }
}