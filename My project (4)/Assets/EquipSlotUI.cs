using UnityEngine;

public class EquipSlotUI : MonoBehaviour
{
    public void SelectPrimarySlot()
    {
        GameManager.Instance.selectedEquipSlot = GameManager.EquipSlot.Primary;
        Debug.Log("Selected PRIMARY slot");
    }

    public void SelectSecondarySlot()
    {
        GameManager.Instance.selectedEquipSlot = GameManager.EquipSlot.Secondary;
        Debug.Log("Selected SECONDARY slot");
    }
}