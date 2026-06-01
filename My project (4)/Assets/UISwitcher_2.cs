using UnityEngine;

public class UISwitcher_2 : MonoBehaviour
{
    [Header("UI To Enable")]
    public GameObject uiToEnable;
    public GameObject uiCursor;

    [Header("UI To Disable")]
    public GameObject uiToDisable;


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
        if (uiToEnable != null) 
        { 
            uiCursor.SetActive(true);
            uiToEnable.SetActive(true);
        }



        if (uiToDisable != null)
            uiToDisable.SetActive(false);    }
}
