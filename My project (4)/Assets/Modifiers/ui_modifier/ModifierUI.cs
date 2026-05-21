using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifierUI : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;

    public TMP_Text nameText;

    public TMP_Text descriptionText;

    public TMP_Text miriumText;

    public ModifierData selectedModifier;

    public void ShowModifier(ModifierData data)
    {
        iconImage.sprite = data.icon;

        nameText.text = data.modifierName;

        descriptionText.text = data.description;

        miriumText.text = "Mirium Needed: " + data.miriumRequired;
    }
}