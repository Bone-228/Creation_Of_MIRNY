using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    public playerHealthManager healthManager;

    void Start()
    {
        if (healthManager == null)
        {
            healthManager = FindObjectOfType<playerHealthManager>();
        }

        if (healthManager != null)
        {
            healthManager.RecalculateHealth();
        }
    }

    public void UnlockModifier(ModifierData modifier)
    {
        if (!GameManager.Instance.unlockedModifiers.Contains(modifier))
        {
            GameManager.Instance.unlockedModifiers.Add(modifier);

            Debug.Log($"Unlocked modifier: {modifier.modifierName}");
        }
    }

    public void EquipModifier(ModifierData modifier)
    {
        if (!GameManager.Instance.equippedModifiers.Contains(modifier))
        {
            GameManager.Instance.equippedModifiers.Add(modifier);

            Debug.Log($"Equipped modifier: {modifier.modifierName}");

            if (healthManager != null)
            {
                healthManager.RecalculateHealth();
            }
        }
    }

    public void UnequipModifier(ModifierData modifier)
    {
        if (GameManager.Instance.equippedModifiers.Contains(modifier))
        {
            GameManager.Instance.equippedModifiers.Remove(modifier);

            Debug.Log($"Unequipped modifier: {modifier.modifierName}");

            if (healthManager != null)
            {
                healthManager.RecalculateHealth();
            }
        }
    }
}