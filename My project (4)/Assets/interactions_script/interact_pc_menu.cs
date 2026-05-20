using Assets.scripts_camera;
using UnityEngine;

public class interact_pc_menu : test_interaction
{
    [Header("UI")]
    public GameObject canvasObject;
    public GameObject secondCanvasObject;
    public GameObject thirdCanvasObject;

    [Header("Player")]
    public PlayerMovement playerMovement;
    public ThirdPersonCam playerCam;

    private bool menuOpen = false;

    private void Update()
    {
        // Close menu with ESC
        if (menuOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public override void Interact()
    {
        // Keep parent interaction behavior
        base.Interact();

        if (canvasObject == null)
        {
            Debug.LogWarning("Canvas object is not assigned.");
            return;
        }

        OpenMenu();
    }

    private void OpenMenu()
    {
        // Enable UI
        canvasObject.SetActive(true);

        if (secondCanvasObject != null)
            secondCanvasObject.SetActive(false);

        if (thirdCanvasObject != null)
            thirdCanvasObject.SetActive(true);

        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Disable camera movement
        if (playerCam != null)
            playerCam.enabled = false;

        // Unlock and show mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuOpen = true;

        Debug.Log("PC menu opened.");
    }

    public void CloseMenu()
    {
        // Disable UI
        if (canvasObject != null)
            canvasObject.SetActive(false);

        if (thirdCanvasObject != null)
            thirdCanvasObject.SetActive(false);

        // Re-enable player movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Re-enable camera movement
        if (playerCam != null)
            playerCam.enabled = true;

        // Lock and hide mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        menuOpen = false;

        Debug.Log("PC menu closed.");
    }
}