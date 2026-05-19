using UnityEngine;
using UnityEngine.UI;

public class healthbar_manager : MonoBehaviour
{
    [Header("References")]
    public playerHealthManager playerHealth;
    public Image healthBarImage;

    void Update()
    {
        if (playerHealth == null || healthBarImage == null)
            return;

        healthBarImage.fillAmount =
            playerHealth.currentHealth / playerHealth.maxHealth;
    }
}