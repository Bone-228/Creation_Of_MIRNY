using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTo_level_one : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }
}
