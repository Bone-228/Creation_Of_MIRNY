using System.Collections;
using UnityEngine;

public class enemy_range_fire : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    public float attackRange = 15f;
    public float fireRate = 1f;
    public float damage = 10f;

    [Header("Animation")]
    public Animator animator;
    public string aimingParameter = "isAiming";
    public string shootParameter = "shoot";

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public float muzzleFlashDelay = 0.05f;

    private bool isFiring = false;
    private Coroutine fireCoroutine;
    private Coroutine muzzleFlashCoroutine;

    public BattleAudioManager battleAudioManager;

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                player = found.transform;
        }
    }

    void Update()
    {
        if (player == null || animator == null)
            return;

        bool inRange = IsPlayerInRange();
        bool isAiming = animator.GetBool(aimingParameter);

        if (inRange && isAiming)
        {
            if (!isFiring)
            {
                fireCoroutine = StartCoroutine(FireLoop());
            }
        }
        else
        {
            StopFiring();
        }
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    private IEnumerator FireLoop()
    {
        isFiring = true;
        battleAudioManager.PlayEnemyShootSound();
        while (player != null &&
               IsPlayerInRange() &&
               animator.GetBool(aimingParameter))
        {
            // STEP 1: SHOOT ON
            animator.SetBool(shootParameter, true);

            // allow animation to enter firing state
            yield return null;

            // STEP 2: EFFECTS + DAMAGE
            PlayMuzzleFlash();
            FireRayDamage();

            // STEP 3: SHOOT OFF
            animator.SetBool(shootParameter, false);

            // STEP 4: WAIT BETWEEN SHOTS
            yield return new WaitForSeconds(fireRate);
        }

        isFiring = false;
        fireCoroutine = null;
    }

    private void FireRayDamage()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            playerHealthManager health =
                hit.collider.GetComponentInParent<playerHealthManager>();

            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("Enemy dealt damage to player");
            }
        }
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash == null) return;

        if (muzzleFlashCoroutine != null)
        {
            StopCoroutine(muzzleFlashCoroutine);
        }

        muzzleFlashCoroutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {
        yield return new WaitForSeconds(muzzleFlashDelay);

        muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        muzzleFlash.Play();
    }

    private void StopFiring()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }

        if (muzzleFlashCoroutine != null)
        {
            StopCoroutine(muzzleFlashCoroutine);
            muzzleFlashCoroutine = null;
        }

        animator.SetBool(shootParameter, false);
        isFiring = false;
    }
}