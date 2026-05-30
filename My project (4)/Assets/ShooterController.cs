using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public event System.Action<Weapon, Weapon> WeaponChanged;

    private List<Weapon> weapons;
    private Weapon currentWeapon;

    public Weapon CurrentWeapon => currentWeapon;

    void Start()
    {
        InitializeWeapons();
    }

    public void InitializeWeapons()
    {
        weapons = GetComponentsInChildren<Weapon>(true).ToList();

        // Disable all weapons first
        foreach (var w in weapons)
            w.gameObject.SetActive(false);

        List<Weapon> equipped = new List<Weapon>();

        foreach (var weapon in weapons)
        {
            if (weapon == null) continue;

            WeaponIdentifier id = weapon.GetComponent<WeaponIdentifier>();
            if (id == null) continue;

            // PRIMARY CHECK
            if (GameManager.Instance.primaryGun != null &&
                id.weaponID == GameManager.Instance.primaryGun.weaponID)
            {
                equipped.Add(weapon);
            }

            // SECONDARY CHECK
            else if (GameManager.Instance.secondaryGun != null &&
                     id.weaponID == GameManager.Instance.secondaryGun.weaponID)
            {
                equipped.Add(weapon);
            }
        }

        weapons = equipped;

        if (weapons.Count == 0)
        {
            Debug.LogWarning("No equipped weapons found!");
            return;
        }

        ChangeWeapon(weapons[0]);
    }

    public void RefreshWeapons()
    {
        InitializeWeapons();
    }

    void Update()
    {
        if (currentWeapon == null)
            return;

        bool fire =
            Input.GetButton("Fire1") ||
            Input.GetMouseButton(0);

        if (fire &&
            currentWeapon.ShootInputMethod != null &&
            currentWeapon.ShootInputMethod("Fire1"))
        {
            currentWeapon.Attack();
        }

        if (Input.GetButtonDown("Reload"))
        {
            currentWeapon.Reload();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count > 0)
            ChangeWeapon(weapons[0]);

        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count > 1)
            ChangeWeapon(weapons[1]);
    }

    private void ChangeWeapon(Weapon newWeapon)
    {
        if (newWeapon == null) return;

        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        Weapon oldWeapon = currentWeapon;
        currentWeapon = newWeapon;

        currentWeapon.gameObject.SetActive(true);

        WeaponChanged?.Invoke(oldWeapon, currentWeapon);
    }
}