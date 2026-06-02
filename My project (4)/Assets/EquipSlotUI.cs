using UnityEngine;

public class EquipSlotUI : MonoBehaviour
{
    public BattleAudioManager battleAudioManager;

    public void SelectPrimarySlot()
    {
        battleAudioManager.PlayButtonClick();
        GameManager.Instance.selectedEquipSlot = GameManager.EquipSlot.Primary;
        Debug.Log("Selected PRIMARY slot");
    }

    public void SelectSecondarySlot()
    {
        battleAudioManager.PlayButtonClick();
        GameManager.Instance.selectedEquipSlot = GameManager.EquipSlot.Secondary;
        Debug.Log("Selected SECONDARY slot");
    }
}