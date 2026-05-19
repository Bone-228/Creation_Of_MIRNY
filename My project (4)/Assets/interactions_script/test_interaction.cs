using UnityEngine;

public class test_interaction : MonoBehaviour, IInteractible
{
    public bool success;
    public virtual void Interact()
    {
        Debug.Log("Interaction successful with " + gameObject.name);
        success = true;
    }
}
