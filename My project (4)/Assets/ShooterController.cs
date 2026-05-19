using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public event Action<Weapon, Weapon> WeaponChanged;

    List<Weapon> weapons;
    Weapon currentWeapon;
    public Weapon CurrentWeapon => currentWeapon;

    void Start()
    {
        weapons = GetComponentsInChildren<Weapon>(true).ToList();
        if (weapons == null || weapons.Count == 0)
        {
            currentWeapon = null;
            return;
        }

        weapons.ForEach(w => w.gameObject.SetActive(false));
        ChangeWeapon(weapons.First());
    }

    void Update()
    {
        if (currentWeapon == null)
            return;

        // Only consider an explicit physical "press" for firing this frame.
        // Using GetButtonDown / GetMouseButtonDown avoids accidental true values coming from other input mappings (e.g. crouch).
        bool physicalFirePressed = Input.GetButton("Fire1") || Input.GetMouseButton(0);

        // Only attempt to attack when both:
        // - a physical fire press occurred this frame
        // - the weapon's input method also indicates it's allowed to fire (preserves any weapon-specific checks)
        if (physicalFirePressed && currentWeapon.ShootInputMethod != null && currentWeapon.ShootInputMethod("Fire1"))
        {
            currentWeapon.Attack();
        }

        if (Input.GetButtonDown("Reload"))
        {
            Debug.Log("Jsem tu!");
            currentWeapon?.Reload();
        }

        // Weapon hotkeys - guard indices to avoid exceptions
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count > 0)
            ChangeWeapon(weapons[0]);

        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count > 1)
            ChangeWeapon(weapons[1]);

        if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Count > 2)
            ChangeWeapon(weapons[2]);

        if (Input.GetKeyDown(KeyCode.G) && weapons.Count > 3)
            ChangeWeapon(weapons[3]);
    }

    private void ChangeWeapon(Weapon newWeapon)
    {
        if (newWeapon == null)
            return;

        if (currentWeapon)
        {
            if (currentWeapon.IsReloading)
                currentWeapon.Reload();
            currentWeapon.gameObject.SetActive(false);
        }

        WeaponChanged?.Invoke(currentWeapon, newWeapon);

        currentWeapon = newWeapon;
        currentWeapon.gameObject.SetActive(true);
    }
}
