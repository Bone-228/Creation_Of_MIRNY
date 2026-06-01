using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    public GameObject pressAnyKeyPanel;
    public GameObject mainMenuPanel;

    private bool started = false;

    void Update()
    {
        if (!started && Input.anyKeyDown)
        {
            started = true;
            StartCoroutine(ShowMainMenu());
        }
    }

    IEnumerator ShowMainMenu()
    {
        yield return null;

        pressAnyKeyPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}