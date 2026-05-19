using UnityEngine;

public class change_interaction_color : MonoBehaviour
{
    public test_interaction inter;

    public Material newMaterial;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (inter.success)
        {
            objectRenderer.material = newMaterial;
        }
    }
}