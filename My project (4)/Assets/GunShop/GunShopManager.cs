using UnityEngine;
using TMPro;
public class GunShopManager : MonoBehaviour
{
    // ───────────────────────── BUY ─────────────────────────
    public GameManager gameManager;
    public TMP_Text playerScrapsText;

    private void Update()
    {
        playerScrapsText.text = GameManager.Instance.playerScraps.ToString();
    }
    public void BuyGun(GunData gun)
    {
        if (gun == null) return;

        if (GameManager.Instance.ownedGuns.Contains(gun))
            return;

        if (GameManager.Instance.playerScraps < gun.gunPrice)
            return;

        GameManager.Instance.playerScraps -= gun.gunPrice;
        GameManager.Instance.ownedGuns.Add(gun);

        Debug.Log("Bought: " + gun.gunName);
    }

    // ───────────────────────── EQUIP (2 SLOT SYSTEM) ─────────────────────────

    public void EquipGun(GunData gun)
    {
        if (gun == null) return;

        if (!GameManager.Instance.ownedGuns.Contains(gun))
            return;

        if (GameManager.Instance.selectedEquipSlot == GameManager.EquipSlot.Primary)
        {
            GameManager.Instance.primaryGun = gun;
            Debug.Log("Equipped PRIMARY: " + gun.gunName);
        }
        else
        {
            GameManager.Instance.secondaryGun = gun;
            Debug.Log("Equipped SECONDARY: " + gun.gunName);
        }

        RefreshShooter();
    }

    private void RefreshShooter()
    {
        ShooterController shooter =
            FindFirstObjectByType<ShooterController>();

        if (shooter != null)
        {
            shooter.RefreshWeapons();
        }
    }
}