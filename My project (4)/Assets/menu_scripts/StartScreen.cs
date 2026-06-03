using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    public GameObject pressAnyKeyPanel;
    public GameObject mainMenuPanel;

    public UIFader fader;

    private bool started = false;

    private void Start()
    {
        // In case we're returning from gameplay
        Time.timeScale = 1f;

        // Reset start screen state
        started = false;

        // Show "Press Any Key"
        if (pressAnyKeyPanel != null)
            pressAnyKeyPanel.SetActive(true);

        // Hide main menu until a key is pressed
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        Debug.Log("StartScreen initialized.");
    }

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
        if (fader != null)
            yield return StartCoroutine(fader.FadeOut(1f));

        pressAnyKeyPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        if (fader != null)
            yield return StartCoroutine(fader.FadeIn(1f));
    }
}