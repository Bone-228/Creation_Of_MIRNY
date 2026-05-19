
using System.Collections;
using UnityEngine;

public class turret_shoot : MonoBehaviour
{
    [Header("Basic")]
    public float damage = 10f;
    public float range = 15f;
    public float fireRate = 1f;
    public Transform firePoint;

    [Header("Effects")]
    [SerializeField]
    public GameObject bulletImpactEffect;

    public ParticleSystem muzzleFlash;

    [Header("Behaviour")]
    [Tooltip("If true the turret will fire automatically at its fire rate.")]
    public bool autoFire = true;

    [Header("Target")]
    public Transform player;

    [Header("Animation")]
    [Tooltip("Animator used for turret animations.")]
    public Animator animator;

    [Header("Animation Parameters")]
    public string firingParameter = "isFiring";
    public string deadParameter = "isDead";
    public string inRangeParameter = "inRange";

    [Header("Animation Speed Settings")]
    [Tooltip("Multiplier applied to the computed animation speed (computed as multiplier / fireRate).")]
    public float animationSpeedMultiplier = 1f;

    [Tooltip("Minimum allowed animation speed.")]
    public float minAnimationSpeed = 0.5f;

    [Tooltip("Maximum allowed animation speed.")]
    public float maxAnimationSpeed = 3f;

    [Header("Muzzle Flash Timing")]
    [Tooltip("Delay (in seconds) after invoking the firing animation before playing the muzzle flash effect.")]
    public float muzzleFlashDelay = 0.05f;

    [Header("Diagnostics")]
    [Tooltip("Last computed shots per second (1 / fireRate).")]
    public float lastShotsPerSecond;

    [Tooltip("Last determined animation duration in seconds (-1 if unknown).")]
    public float lastAnimationDuration = -1f;

    protected float fireTimer;

    float defaultAnimatorSpeed = 1f;

    Coroutine muzzleFlashCoroutine;
    Coroutine shootChainCoroutine;

    bool isDead = false;

    protected virtual void Start()
    {
        fireTimer = 0f;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            defaultAnimatorSpeed = animator.speed;
        }

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }
    }

    protected virtual void Update()
    {
        if (isDead)
            return;

        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
        }

        bool playerInRange = IsPlayerInRange();

        // Update animator inRange parameter
        UpdateInRangeAnimation(playerInRange);

        if (animator != null)
        {
            bool isActive = playerInRange && (autoFire || fireTimer > 0f);

            if (isActive)
            {
                float denom = Mathf.Max(0.0001f, fireRate);
                float computed = animationSpeedMultiplier / denom;
                float clamped = Mathf.Clamp(computed, minAnimationSpeed, maxAnimationSpeed);

                animator.speed = clamped * defaultAnimatorSpeed;
            }
            else
            {
                animator.speed = defaultAnimatorSpeed;
            }
        }

        bool shouldFire = autoFire && playerInRange;

        if (shouldFire && fireTimer <= 0f)
        {
            Shoot();
        }
        else
        {
            UpdateFiringAnimation(false);
        }
    }

    private bool IsPlayerInRange()
    {
        if (player == null)
            return false;

        float distance = Vector3.Distance(transform.position, player.position);

        return distance <= range;
    }

    public void Attack()
    {
        if (isDead)
            return;

        if (CanAttack() && IsPlayerInRange())
        {
            Shoot();
        }
    }

    public virtual bool CanAttack()
    {
        return !isDead && fireTimer <= 0f;
    }

    void UpdateFiringAnimation(bool firing)
    {
        if (animator != null)
        {
            animator.SetBool(firingParameter, firing);
        }
    }

    void UpdateInRangeAnimation(bool inRange)
    {
        if (animator != null)
        {
            animator.SetBool(inRangeParameter, inRange);
        }
    }

    protected void Shoot()
    {
        if (!CanAttack())
            return;

        UpdateFiringAnimation(true);

        ComputeFireRateAndAnimationDuration();

        DoShoot();

        float nextDelay = lastAnimationDuration > 0f
            ? lastAnimationDuration
            : Mathf.Max(0.0001f, fireRate);

        fireTimer = nextDelay;

        if (autoFire)
        {
            if (shootChainCoroutine != null)
            {
                StopCoroutine(shootChainCoroutine);
                shootChainCoroutine = null;
            }

            shootChainCoroutine = StartCoroutine(ShootChainAfterDelay(nextDelay));
        }
    }

    protected virtual void DoShoot()
    {
        if (muzzleFlash != null)
        {
            if (muzzleFlashCoroutine != null)
            {
                StopCoroutine(muzzleFlashCoroutine);
                muzzleFlashCoroutine = null;
            }

            muzzleFlashCoroutine = StartCoroutine(PlayMuzzleFlashAfterDelay());
        }

        if (firePoint == null)
        {
            Debug.LogWarning("turret_shoot: firePoint is not assigned.");
            return;
        }

        Ray ray = new Ray(firePoint.position, firePoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Turret hit: " + hit.collider.name);

            if (bulletImpactEffect != null)
            {
                GameObject impact = Instantiate(
                    bulletImpactEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );

                Destroy(impact, 2f);
            }

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForceAtPosition(
                    ray.direction * 5f,
                    hit.point,
                    ForceMode.Impulse
                );
            }

            playerHealthManager playerHealth =
                hit.collider.GetComponentInParent<playerHealthManager>();

            if (playerHealth != null)
            {
                Debug.Log($"Turret dealt {damage} damage to player.");

                playerHealth.TakeDamage(damage);
            }
        }
    }

    IEnumerator ShootChainAfterDelay(float delay)
    {
        yield return new WaitForSeconds(Mathf.Max(0f, delay));

        shootChainCoroutine = null;

        UpdateFiringAnimation(false);

        if (autoFire && IsPlayerInRange() && CanAttack())
        {
            Shoot();
        }
    }

    void ComputeFireRateAndAnimationDuration()
    {
        lastShotsPerSecond = 1f / Mathf.Max(0.0001f, fireRate);

        lastAnimationDuration = -1f;

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            var clips = animator.runtimeAnimatorController.animationClips;

            AnimationClip selected = null;

            foreach (var clip in clips)
            {
                if (clip == null)
                    continue;

                var n = clip.name.ToLowerInvariant();

                if (n.Contains("shoot") || n.Contains("fire"))
                {
                    selected = clip;
                    break;
                }
            }

            if (selected == null && clips.Length > 0)
            {
                selected = clips[0];
            }

            if (selected != null)
            {
                float speed = Mathf.Max(0.0001f, animator.speed);

                lastAnimationDuration = selected.length / speed;
            }
        }

        if (lastAnimationDuration < 0f)
        {
            lastAnimationDuration = 1f / Mathf.Max(0.0001f, lastShotsPerSecond);
        }

        Debug.Log(
            $"Turret firing: shotsPerSecond={lastShotsPerSecond:F2}, animationDuration={lastAnimationDuration:F2}s"
        );
    }

    IEnumerator PlayMuzzleFlashAfterDelay()
    {
        float delay = Mathf.Max(0f, muzzleFlashDelay);

        if (delay > 0f)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        if (muzzleFlash != null)
        {
            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            muzzleFlash.Play();
        }

        muzzleFlashCoroutine = null;
    }

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash == null)
            return;

        if (muzzleFlashCoroutine != null)
        {
            StopCoroutine(muzzleFlashCoroutine);
            muzzleFlashCoroutine = null;
        }

        muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        muzzleFlash.Play();
    }

    public void PlayMuzzleFlashWithDelay(float delay)
    {
        if (muzzleFlash == null)
            return;

        if (muzzleFlashCoroutine != null)
        {
            StopCoroutine(muzzleFlashCoroutine);
            muzzleFlashCoroutine = null;
        }

        muzzleFlashCoroutine = StartCoroutine(DelayedPlay(Mathf.Max(0f, delay)));
    }

    IEnumerator DelayedPlay(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        if (muzzleFlash != null)
        {
            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            muzzleFlash.Play();
        }

        muzzleFlashCoroutine = null;
    }

    public void KillTurret()
    {
        if (isDead)
            return;

        isDead = true;

        UpdateFiringAnimation(false);
        UpdateInRangeAnimation(false);

        if (animator != null)
        {
            animator.SetBool(deadParameter, true);
        }

        if (shootChainCoroutine != null)
        {
            StopCoroutine(shootChainCoroutine);
            shootChainCoroutine = null;
        }

        if (muzzleFlashCoroutine != null)
        {
            StopCoroutine(muzzleFlashCoroutine);
            muzzleFlashCoroutine = null;
        }

        Debug.Log("Turret entered DEAD state.");
    }
}

