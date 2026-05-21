using UnityEngine;

public class ModifierButton : MonoBehaviour
{
    public ModifierData modifierData;
    public ModifierUI modifierUI;

    public void SelectModifier()
    {
        modifierUI.ShowModifier(modifierData);

        modifierUI.selectedModifier = modifierData;
    }
}