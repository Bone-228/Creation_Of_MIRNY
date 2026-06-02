using UnityEditor.UI;
using UnityEngine;

public class level_select_menu_change : MonoBehaviour
{
    public GameObject canvasToDisable;
    public GameObject canvasToEnable;

    public AudioManager audioManager;

    public void SwitchCanvas()
    {
        canvasToDisable.SetActive(false);
        canvasToEnable.SetActive(true);

        audioManager.PlayButtonClick();
    }
}
