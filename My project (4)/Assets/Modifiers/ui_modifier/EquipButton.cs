using UnityEngine;

public class EquipButton : MonoBehaviour
{
    public ModifierUI modifierUI;
    public ModifierManager modifierManager;

    public void EquipSelected()
    {
        ModifierData mod = modifierUI.selectedModifier;

        if (mod == null)
        {
            Debug.Log("No modifier selected!");
            return;
        }

        modifierManager.EquipModifier(mod);
        Debug.Log($"Equipped modifier: {mod.modifierName}");
    }
}