using UnityEngine;

public class GunShopManager : MonoBehaviour
{
    // ───────────────────────── BUY GUN ─────────────────────────

    public void BuyGun(GunData gun)
    {
        if (gun == null)
            return;

        // Already owned
        if (GameManager.Instance.ownedGuns.Contains(gun))
        {
            Debug.Log("Gun already owned.");
            return;
        }

        // Not enough money
        if (GameManager.Instance.playerScraps < gun.gunPrice)
        {
            Debug.Log("Not enough currency.");
            return;
        }

        GameManager.Instance.playerScraps -= gun.gunPrice;

        GameManager.Instance.ownedGuns.Add(gun);

        Debug.Log("Bought gun: " + gun.gunName);
    }

    // ───────────────────────── EQUIP PRIMARY ─────────────────────────

    public void EquipPrimary(GunData gun)
    {
        if (gun == null)
            return;

        // Must own gun
        if (!GameManager.Instance.ownedGuns.Contains(gun))
        {
            Debug.Log("Gun not owned.");
            return;
        }

        // Prevent duplicate equip
        if (GameManager.Instance.secondaryGun == gun)
        {
            Debug.Log("Already equipped in secondary slot.");
            return;
        }

        GameManager.Instance.primaryGun = gun;

        Debug.Log("Primary equipped: " + gun.gunName);
    }

    // ───────────────────────── EQUIP SECONDARY ─────────────────────────

    public void EquipSecondary(GunData gun)
    {
        if (gun == null)
            return;

        if (!GameManager.Instance.ownedGuns.Contains(gun))
        {
            Debug.Log("Gun not owned.");
            return;
        }

        if (GameManager.Instance.primaryGun == gun)
        {
            Debug.Log("Already equipped in primary slot.");
            return;
        }

        GameManager.Instance.secondaryGun = gun;

        Debug.Log("Secondary equipped: " + gun.gunName);
    }
}