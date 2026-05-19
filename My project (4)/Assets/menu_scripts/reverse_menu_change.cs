using UnityEngine;

public class reverse_menu_change : MonoBehaviour
{
    public GameObject canvasToDisable;
    public GameObject canvasToEnable;

    public void SwitchCanvas()
    {
        canvasToDisable.SetActive(false);
        canvasToEnable.SetActive(true);
    }
}
