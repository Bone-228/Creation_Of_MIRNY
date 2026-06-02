using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public Image fadeImage;

    public IEnumerator FadeOut(float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            Color c = fadeImage.color;
            c.a = Mathf.Lerp(0, 1, t / duration);

            fadeImage.color = c;

            yield return null;
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            Color c = fadeImage.color;
            c.a = Mathf.Lerp(1, 0, t / duration);

            fadeImage.color = c;

            yield return null;
        }
    }
}