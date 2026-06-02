using UnityEditor;
using UnityEngine;

public class quit_button_script : MonoBehaviour
{
    public AudioManager audioManager;


    public void QuitGame()
    {
        Debug.Log("Quitting game");
        audioManager.PlayButtonClick();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
