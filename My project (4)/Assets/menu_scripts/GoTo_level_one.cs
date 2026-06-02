using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTo_level_one : MonoBehaviour
{
    public AudioManager audioManager;

    public void LoadScene(string sceneName)
    {
        audioManager.PlayButtonClick();
        SceneManager.LoadScene(sceneName);
    }
}
