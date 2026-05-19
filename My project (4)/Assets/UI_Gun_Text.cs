using TMPro;
using UnityEngine;

public class UI_Gun_Text : MonoBehaviour
{
    [SerializeField] ShooterController shooterController;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI weaponNameText;
    [SerializeField] TextMeshProUGUI ammoText;

    Weapon currentWeapon;

    void Start()
    {
        if (shooterController == null)
            shooterController = FindObjectOfType<ShooterController>();

        shooterController.WeaponChanged += OnWeaponChanged;

        if (shooterController.CurrentWeapon != null)
            OnWeaponChanged(null, shooterController.CurrentWeapon);
    }

    void Update()
    {
        if (currentWeapon == null)
            return;

        UpdateAmmoUI();
    }

    void OnWeaponChanged(Weapon oldWeapon, Weapon newWeapon)
    {
        currentWeapon = newWeapon;

        if (currentWeapon == null)
            return;

        weaponNameText.text = currentWeapon.name;

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        ammoText.text = $"{currentWeapon.Ammo} / {currentWeapon.MaxAmmo}";
    }

    void OnDestroy()
    {
        if (shooterController != null)
            shooterController.WeaponChanged -= OnWeaponChanged;
    }
}