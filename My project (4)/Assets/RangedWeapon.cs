using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedWeapon : Weapon
{
    [SerializeField]
    protected int maxAmmo = 7;
    protected int currentAmmo;

    public override int Ammo => currentAmmo;
    public override int MaxAmmo => maxAmmo;

    public override bool IsReloading => remainingReloadTime > 0;

    [Header("Audios")]
    public BattleAudioManager battleAudioManager;
    public AudioClip shootSound;
    public AudioClip reloadSound;


    [SerializeField]
    protected float bulletRange = 100f;

    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    protected float reloadTime;
    protected float remainingReloadTime;

    [SerializeField]
    public GameObject bulletImpactEffect;

    [SerializeField]
    public ParticleSystem muzzleFlash;

    [Header("Bullet Trail")]
    [SerializeField]
    private TrailRenderer bulletTrailPrefab;

    [SerializeField]
    private float trailSpeed = 250f;

    [Header("UI Crosshair (optional)")]
    [SerializeField]
    protected Canvas uiCanvas;

    [SerializeField]
    protected RectTransform crosshair;

    [SerializeField]
    protected Camera playerCamera;

    [Header("Hit Marker UI")]
    [SerializeField]
    private GameObject hitMarkerObject;

    [SerializeField]
    private float hitMarkerShowTime = 0.1f;

    private Coroutine hitMarkerRoutine;

    protected virtual void Start()
    {
        ChangeAmmo(maxAmmo);
        ShootInputMethod = Input.GetButtonDown;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        HideHitMarker();
    }

    public override void Reload()
    {
        battleAudioManager.PlaySelectedSound(reloadSound);
        remainingReloadTime = reloadTime;
        RaiseReloadProgressChanged(1);
        Debug.Log($"Reloading {remainingReloadTime}");
    }

    protected override void Update()
    {
        base.Update();

        if (remainingReloadTime > 0)
        {
            remainingReloadTime -= Time.deltaTime;
            RaiseReloadProgressChanged(remainingReloadTime / reloadTime);

            if (remainingReloadTime <= 0)
            {
                ChangeAmmo(maxAmmo);
            }
        }
    }

    protected void ChangeAmmo(int newVal)
    {
        currentAmmo = newVal;
        RaiseAmmoChanged();
    }

    public void Shoot()
    {
        if (IsReloading)
            return;

        ChangeAmmo(currentAmmo - 1);

        battleAudioManager.PlaySelectedSound(shootSound);

        DoShoot();

        fireTimer = fireRate;

        if (currentAmmo <= 0)
        {
            Reload();
        }
    }

    protected virtual void DoShoot()
    {
        muzzleFlash?.Play();
        Vector2 screenPoint;
        if (crosshair != null)
        {
            Camera rectCamera = null;
            if (uiCanvas != null && uiCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                rectCamera = uiCanvas.worldCamera;
            }
            screenPoint = RectTransformUtility.WorldToScreenPoint(rectCamera,crosshair.position);
        }
        else
        {
            screenPoint = new Vector2(Screen.width * 0.5f,Screen.height * 0.5f);
        }
        Camera cam = playerCamera != null ? playerCamera : Camera.main;
        Ray ray = cam.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        Vector3 trailTarget;
        if (Physics.Raycast(ray, out hit, bulletRange))
        {
            Debug.Log("We hit " + hit.collider.name);
            trailTarget = hit.point;
            if (bulletImpactEffect != null)
            {
                GameObject impact = Instantiate(bulletImpactEffect,hit.point, Quaternion.LookRotation(hit.normal));
                impact.transform.SetParent(hit.collider.transform, true);
                Destroy(impact, 2f);
            }
            // Rigidbody force
            var rb = hit.collider.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddForceAtPosition(ray.direction * 5f,hit.point,ForceMode.Impulse);
            }
            // Regular hitbox damage
            var hitbox = hit.collider.GetComponent<HitBox>();
            if (hitbox)
            {
                hitbox.OnRayCastHit(this);
                ShowHitMarker();
            }
            // Turret destruction logic
            if (hit.collider.CompareTag("turretDamage"))
            {
                HitBoxTurret turretHitbox = hit.collider.GetComponent<HitBoxTurret>();
                if (turretHitbox != null)
                {
                    turretHitbox.HitByRaycast();
                    ShowHitMarker();
                }
            }
        }
        else
        {
            trailTarget = ray.origin + ray.direction * bulletRange;
        }
        // Bullet trail
        if (bulletTrailPrefab != null && firePoint != null)
        {
            TrailRenderer trail = Instantiate(bulletTrailPrefab,firePoint.position,Quaternion.identity);
            StartCoroutine(AnimateTrail(trail, trailTarget));
        }
    }

    private IEnumerator AnimateTrail(
        TrailRenderer trail,
        Vector3 targetPoint)
    {
        Vector3 startPoint = trail.transform.position;

        float distance = Vector3.Distance(startPoint, targetPoint);
        float duration = distance / trailSpeed;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            trail.transform.position = Vector3.Lerp(
                startPoint,
                targetPoint,
                t
            );

            yield return null;
        }

        trail.transform.position = targetPoint;

        Destroy(trail.gameObject, trail.time);
    }

    public void ShowHitMarker()
    {
        if (hitMarkerObject == null)
            return;

        if (hitMarkerRoutine != null)
        {
            StopCoroutine(hitMarkerRoutine);
        }

        hitMarkerRoutine = StartCoroutine(HitMarkerRoutine());
    }

    private IEnumerator HitMarkerRoutine()
    {
        battleAudioManager.PlayEnemyHitSound();
        hitMarkerObject.SetActive(true);

        yield return new WaitForSeconds(hitMarkerShowTime);

        HideHitMarker();
    }

    private void HideHitMarker()
    {
        if (hitMarkerObject != null)
        {
            hitMarkerObject.SetActive(false);
        }
    }

    public override void Attack()
    {
        if (CanAttack())
            Shoot();
    }

    public override bool CanAttack()
    {
        return base.CanAttack() && currentAmmo > 0;
    }
}