using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [Header("UI To Enable")]
    public GameObject uiToEnable;

    [Header("UI To Disable")]
    public GameObject uiToDisable;
    public GameObject uiCursor;

    public BattleAudioManager battleAudioManager;

    void Update()
    {
        // Press ESC to switch UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchUI();
        }
    }

    // Call this from your Button
    public void SwitchUI()
    {
        battleAudioManager.PlayButtonClick();

        if (uiToEnable != null)
            uiToEnable.SetActive(true);

        if (uiToDisable != null)
            uiToDisable.SetActive(false);
            uiCursor.SetActive(false);
    }
}
