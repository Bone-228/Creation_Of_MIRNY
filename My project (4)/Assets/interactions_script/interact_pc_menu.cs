
using UnityEngine;

public class interact_pc_menu : test_interaction
{
    [Header("UI")]
    public GameObject canvasObject;
    public GameObject secondCanvasObject; // Unused, consider removing or implementing if needed

    public override void Interact()
    {
        // Keep parent interaction behavior
        base.Interact();

        if (canvasObject != null)
        {
            canvasObject.SetActive(true);
            secondCanvasObject.SetActive(false); // Activate second canvas if assigned
            Debug.Log("PC menu opened.");
        }
        else
        {
            Debug.LogWarning("Canvas object is not assigned.");
        }
    }
}

