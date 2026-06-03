using UnityEngine;

public class playerHealthManager : MonoBehaviour
{
    [Header("Base Health")]
    public float baseMaxHealth = 100f;

    [Header("Runtime Health")]
    public float maxHealth;
    public float currentHealth;

    private bool isDead = false;

    public BattleAudioManager battleAudioManager;
    void Start()
    {
        RecalculateHealth();

        currentHealth = maxHealth;
    }

    void Update()
    {
        checkDeath();
    }

    public void RecalculateHealth()
    {
        float oldMaxHealth = maxHealth;

        // Reset max health to base
        maxHealth = baseMaxHealth;

        // Apply all equipped modifiers
        foreach (ModifierData modifier in GameManager.Instance.equippedModifiers)
        {
            ExtendHealthModifier healthModifier =
                modifier as ExtendHealthModifier;

            if (healthModifier != null)
            {
                maxHealth += healthModifier.healthBonus;
            }
        }

        // OPTION 3: reward player when max health increases
        float difference = maxHealth - oldMaxHealth;

        if (difference > 0)
        {
            currentHealth += difference;
        }

        // Safety clamp
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        battleAudioManager.PlayPlayerHitSound();

        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"Player took {damage} damage. Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"Player healed {amount}. Health: {currentHealth}");
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player died!");
    }

    private void checkDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
}