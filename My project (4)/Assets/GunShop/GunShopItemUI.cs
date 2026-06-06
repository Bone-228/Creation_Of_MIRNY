using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunShopItemUI : MonoBehaviour
{
    public GunData gunData;

    [Header("UI")]
    public TMP_Text gunNameText;
    public TMP_Text gunPriceText;


    public Button actionButton;
    public TMP_Text buttonText;

    public GunShopManager shopManager;



    void Start()
    {
        shopManager = FindFirstObjectByType<GunShopManager>();
        actionButton.onClick.AddListener(OnClick);
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (gunData == null) return;

        gunNameText.text = gunData.gunName;
        gunPriceText.text = "Price: " + gunData.gunPrice;


        bool owned = GameManager.Instance.OwnsGun(gunData);

        bool isPrimary = GameManager.Instance.primaryGun == gunData;
        bool isSecondary = GameManager.Instance.secondaryGun == gunData;

        if (!owned)
        {
            buttonText.text = "BUY";
        }
        else if (isPrimary)
        {
            buttonText.text = "EQUIPPED (P)";
        }
        else if (isSecondary)
        {
            buttonText.text = "EQUIPPED (S)";
        }
        else
        {
            buttonText.text = "EQUIP";
        }
    }

    private void OnClick()
    {
        bool owned = GameManager.Instance.OwnsGun(gunData);

        if (!owned)
        {
            shopManager.BuyGun(gunData);
        }
        else
        {
            shopManager.EquipGun(gunData);
        }

        RefreshAllUI();
    }

    private void RefreshAllUI()
    {
        GunShopItemUI[] all =
            FindObjectsOfType<GunShopItemUI>();

        foreach (var ui in all)
        {
            ui.RefreshUI();
        }
    }
}