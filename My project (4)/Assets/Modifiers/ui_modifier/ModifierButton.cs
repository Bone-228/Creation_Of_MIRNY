using UnityEngine;

public class ModifierButton : MonoBehaviour
{
    public ModifierData modifierData;
    public ModifierUI modifierUI;

    public BattleAudioManager battleAudioManager;

    public void SelectModifier()
    {
        battleAudioManager.PlayButtonClick();

        modifierUI.ShowModifier(modifierData);

        modifierUI.selectedModifier = modifierData;
    }
}