using System;
using UnityEngine;

public class weapon_animation_controller : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField]
    private Animator playerAnimator;

    [Header("Aiming Layer Settings")]
    [SerializeField]
    private string aimLayerName = "WeaponAiming";

    [SerializeField]
    private int aimLayerIndex = 1;

    [Range(0f, 1f)]
    [SerializeField]
    private float aimWeight = 1f;

    [SerializeField]
    private float transitionSpeed = 8f;

    [Header("Aim Detection")]
    [SerializeField]
    private detectAiming detectAimingScript;

    [SerializeField]
    private string aimParamName = "isAiming";

    [SerializeField]
    public int specificId = 2;

    // ---------------------------
    // Weapon Animation Settings
    // ---------------------------
    [Header("Weapon Animation Settings")]
    [SerializeField] private string weaponTypeParam = "weaponType";
    [SerializeField] private string weaponEquippedParam = "weaponEquiped";
    [SerializeField] private string weaponLayerName = "Weapon";
    [SerializeField] private int weaponLayerIndex = 2;

    private ShooterController shooterController;

    // Internal state
    private float currentWeight = 0f;
    private bool isAiming = false;
    private bool hasValidAnimator = false;
    private bool hasDetectAiming = false;

    void Start()
    {
        // Animator setup (UNCHANGED)
        if (playerAnimator == null)
            playerAnimator = GetComponentInParent<Animator>();

        hasValidAnimator = playerAnimator != null;

        if (hasValidAnimator)
        {
            // Resolve aiming layer
            int idx = playerAnimator.GetLayerIndex(aimLayerName);
            if (idx >= 0)
                aimLayerIndex = idx;

            currentWeight = playerAnimator.GetLayerWeight(aimLayerIndex);

            // Resolve weapon layer
            idx = playerAnimator.GetLayerIndex(weaponLayerName);
            if (idx >= 0)
                weaponLayerIndex = idx;

            // Make sure weapon layer starts disabled
            playerAnimator.SetLayerWeight(weaponLayerIndex, 0f);
        }

        // detectAiming setup (UNCHANGED)
        detectAimingScript = GetComponentInParent<detectAiming>();
        hasDetectAiming = detectAimingScript != null;

        // ShooterController setup
        shooterController = GetComponentInParent<ShooterController>();

        if (shooterController != null)
        {
            shooterController.WeaponChanged += OnWeaponChanged;
        }
        else
        {
            Debug.LogWarning("weapon_animation_controller: No ShooterController found.");
        }
    }

    void Update()
    {
        // Preserve detect aiming logic
        if (hasDetectAiming && detectAimingScript != null)
            SetAiming(detectAimingScript.isAiming);

        // Need a valid animator to set parameters
        if (!hasValidAnimator)
            return;

        // Keep updating the animator aim boolean
        playerAnimator.SetBool(aimParamName, isAiming);

        // Require shooterController to decide layer weight based on current weapon
        if (shooterController == null)
            return;

        Weapon currentWeapon = shooterController.CurrentWeapon;

        if (currentWeapon != null && currentWeapon.Id != 0 && currentWeapon.Id == specificId)
        {
            // Weapon equipped and not empty -> enable aiming layer
            playerAnimator.SetLayerWeight(aimLayerIndex, 1f);
        }
        else
        {
            // No weapon or empty weapon -> disable aiming layer
            playerAnimator.SetLayerWeight(aimLayerIndex, 0f);
        }
    }

    // 🔥 THIS RUNS EXACTLY WHEN ChangeWeapon() IS CALLED
    private void OnWeaponChanged(Weapon oldWeapon, Weapon newWeapon)
    {
        if (!hasValidAnimator)
            return;

        if (newWeapon == null)
        {
            playerAnimator.SetBool(weaponEquippedParam, false);
            playerAnimator.SetLayerWeight(weaponLayerIndex, 0f);
            return;
        }

        // Set weapon type
        playerAnimator.SetFloat(weaponTypeParam, newWeapon.Id);

        // Set equipped bool
        playerAnimator.SetBool(weaponEquippedParam, true);

        // 🔥 Immediately activate weapon layer
        playerAnimator.SetLayerWeight(weaponLayerIndex, 1f);
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }

    public bool IsAiming => isAiming;

    private void OnDestroy()
    {
        if (shooterController != null)
            shooterController.WeaponChanged -= OnWeaponChanged;
    }
}
