using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enemy_health enemyHealth;

    public void OnRayCastHit(Weapon weapon)
    {
        // If this collider is marked as noDamage, do not apply any damage.
        if (gameObject.CompareTag("noDamage"))
        {
            return;
        }

        // Defensive checks to avoid NullReferenceExceptions.
        if (weapon == null || enemyHealth == null)
        {
            return;
        }

        enemyHealth.TakeDamage(weapon.damage);
    }
}
