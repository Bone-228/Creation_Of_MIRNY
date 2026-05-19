using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HitMakerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject hitMarkerObject;

    [SerializeField]
    private float showTime = 0.1f;

    private Coroutine currentRoutine;

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        hitMarkerObject.SetActive(true);

        yield return new WaitForSeconds(showTime);

        Hide();
    }

    private void Hide()
    {
        hitMarkerObject.SetActive(false);
    }
}
