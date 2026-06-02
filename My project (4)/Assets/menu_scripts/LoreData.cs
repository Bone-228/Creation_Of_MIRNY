using UnityEngine;

[CreateAssetMenu(
    fileName = "New Lore",
    menuName = "Lore/Lore Entry"
)]
public class LoreData : ScriptableObject
{
    public string title;

    [TextArea(15, 50)]
    public string loreText;
}