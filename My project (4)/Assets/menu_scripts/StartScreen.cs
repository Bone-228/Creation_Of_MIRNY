using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    public GameObject pressAnyKeyPanel;
    public GameObject mainMenuPanel;

    public UIFader fader;

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
        yield return StartCoroutine(fader.FadeOut(1f));

        pressAnyKeyPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        yield return StartCoroutine(fader.FadeIn(1f));
    }
}