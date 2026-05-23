using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    public playerHealthManager healthManager;
    public PlayerMovement playerMovement;
    void Start()
    {
        if (healthManager == null)
        {
            healthManager = FindObjectOfType<playerHealthManager>();
        }

        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
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

            if (playerMovement != null)
            {
                playerMovement.RecalculateMovement();
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

            if (playerMovement != null)
            {
                playerMovement.RecalculateMovement();
            }
        }
    }

    public void OnEnemyKilled()
    {
        if (healthManager == null)
            return;

        foreach (ModifierData modifier in GameManager.Instance.equippedModifiers)
        {
            lifestealerModifier lifestealModifier =
                modifier as lifestealerModifier;

            if (lifestealModifier != null)
            {
                healthManager.Heal(lifestealModifier.healAmount);

                Debug.Log(
                    $"Lifestealer healed player for " +
                    $"{lifestealModifier.healAmount}"
                );
            }
        }
    }
}