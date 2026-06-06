using Assets.scripts_camera;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenuTriger : MonoBehaviour
{
    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    public PlayerMovement playerMovement;
    public ThirdPersonCam playerCamera;
    public GameObject uiCursor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.enabled = false; // Disable player movement
            playerCamera.enabled = false; // Disable player camera control
            uiCursor.SetActive(true); // Show cursor
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = false; // Make cursor visible
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}