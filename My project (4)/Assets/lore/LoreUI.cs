using TMPro;
using UnityEngine;

public class LoreUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI loreText;

    public void ShowLore(LoreData lore)
    {
        if (lore == null)
            return;

        titleText.text = lore.title;

        loreText.text = lore.loreText;
    }
}