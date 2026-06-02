using UnityEngine;

public class reverse_menu_change : MonoBehaviour
{
    public GameObject canvasToDisable;
    public GameObject canvasToEnable;

    public AudioManager audioManager;
    public void SwitchCanvas()
    {
        audioManager.PlayButtonClick();
        canvasToDisable.SetActive(false);
        canvasToEnable.SetActive(true);
    }
}
