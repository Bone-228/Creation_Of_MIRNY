using UnityEditor;
using UnityEngine;

public class quit_button_script : MonoBehaviour
{



    public void QuitGame()
    {
        Debug.Log("Quitting game");


#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
