using UnityEngine;

public class detectAiming : MonoBehaviour
{
    // Public flag indicating whether the player is currently aiming (holding right mouse button).
    public bool isAiming;

    // Start is called once before the first frame update
    void Start()
    {
        isAiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Right mouse button index is 1; GetMouseButton returns true while held.
        isAiming = Input.GetMouseButton(1);
    }
}
