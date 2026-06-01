using System.Collections.Generic;
using UnityEngine;

public class Shotgun : RangedWeapon
{
    [Header("Shotgun")]
    [SerializeField]
    private int pelletCount = 8;

    [SerializeField]
    private float spreadAngle = 6f;

    [SerializeField]
    private float pelletDamage = 5f;

    protected override void DoShoot()
    {
        muzzleFlash?.Play();

        Camera cam =
            playerCamera != null
            ? playerCamera
            : Camera.main;

        Vector2 centerScreen =
            new Vector2(
                Screen.width * 0.5f,
                Screen.height * 0.5f
            );

        // Count pellets per target
        Dictionary<HitBox, int> pelletHits =
            new Dictionary<HitBox, int>();

        for (int i = 0; i < pelletCount; i++)
        {
            ShootPellet(
                cam,
                centerScreen,
                pelletHits
            );
        }

        // Apply damage once per target
        foreach (var pair in pelletHits)
        {
            HitBox hitbox = pair.Key;
            int hitCount = pair.Value;

            float totalDamage =
                hitCount * pelletDamage;

            Debug.Log(
                $"{hitbox.name} hit by {hitCount} pellets " +
                $"for {totalDamage} damage"
            );

            // If your HitBox already uses weapon damage:
            for (int i = 0; i < hitCount; i++)
            {
                hitbox.OnRayCastHit(this);
            }
        }
    }

    private void ShootPellet(
        Camera cam,
        Vector2 centerScreen,
        Dictionary<HitBox, int> pelletHits)
    {
        Ray ray =
            cam.ScreenPointToRay(centerScreen);

        Vector3 direction =
            Quaternion.Euler(
                Random.Range(
                    -spreadAngle,
                    spreadAngle
                ),
                Random.Range(
                    -spreadAngle,
                    spreadAngle
                ),
                0
            ) * ray.direction;

        RaycastHit hit;

        if (Physics.Raycast(
            ray.origin,
            direction,
            out hit,
            bulletRange))
        {
            HitBox hitbox =
                hit.collider.GetComponent<HitBox>();

            if (hitbox != null)
            {
                if (!pelletHits.ContainsKey(hitbox))
                {
                    pelletHits.Add(hitbox, 0);
                }

                pelletHits[hitbox]++;
            }

            ShowHitMarker();

            // Impact effect
            if (bulletImpactEffect != null)
            {
                GameObject impact =
                    Instantiate(
                        bulletImpactEffect,
                        hit.point,
                        Quaternion.LookRotation(
                            hit.normal
                        )
                    );

                impact.transform.SetParent(
                    hit.collider.transform,
                    true
                );

                Destroy(impact, 2f);
            }
        }
    }
}