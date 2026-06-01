using UnityEngine;

public class UIMenuToggle : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiBattle;
    public GameObject uiMenu;
    public GameObject uiCursor;

    [Header("Player (optional but recommended)")]
    public MonoBehaviour playerMovement;
    public MonoBehaviour playerCamera;

    private bool menuOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOpen)
                CloseMenu();
            else
                OpenMenu();
        }
    }

    void OpenMenu()
    {
        menuOpen = true;

        uiBattle.SetActive(false);
        uiMenu.SetActive(true);
        uiCursor.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerCamera != null)
            playerCamera.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseMenu()
    {
        menuOpen = false;

        uiBattle.SetActive(true);
        uiMenu.SetActive(false);
        uiCursor.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerCamera != null)
            playerCamera.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}