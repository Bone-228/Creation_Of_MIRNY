    using UnityEngine;

    // Pseudocode / Plan:
    // 1. Ensure there is a public interface `IInteractible` other scripts can implement.
    // 2. In the interactor MonoBehaviour:
    //    - Expose `interactionSource` (allow it to be assigned in inspector).
    //    - In Start(): if `interactionSource` is null, try to use the attached transform (if this script is on the camera).
    //      If still null, try Camera.main.transform as a fallback.
    //    - On Update(): when the "E" key is pressed:
    //        - Create a Ray from `interactionSource.position` along `interactionSource.forward`.
    //        - Draw a debug ray for visualization.
    //        - Perform Physics.Raycast with the configured range.
    //        - If a hit occurs, get all MonoBehaviour components on the hit object and find the first that implements `IInteractible`.
    //        - If found, call `Interact()` on that interface.
    //    - Add useful Debug.Log messages and null checks to help diagnose issues when the script is not on the camera or the interactionSource is not set.
    //
    // Notes:
    // - Unity's generic TryGetComponent<T>() requires T : Component and doesn't work with interfaces directly, so we inspect MonoBehaviour components and test for the interface.
    // - This script is safe to attach to the camera; it will default to the camera's transform if `interactionSource` isn't assigned.

    public interface IInteractible
    {
        void Interact();
    }

    public class interactor : MonoBehaviour
    {
        [Tooltip("Source transform for the interaction ray. If null, will use this object's transform or Camera.main.")]
        public Transform interactionSource;

        [Tooltip("Maximum distance for interaction raycast.")]
        public float interactionRange = 3f;

        // Optional: layer mask to filter raycasts (set in inspector if needed)
        public LayerMask interactionMask = Physics.DefaultRaycastLayers;

        void Start()
        {
            // If not assigned, prefer the transform this script is on (e.g., camera).
            if (interactionSource == null && transform != null)
            {
                interactionSource = transform;
            }

            // If still null, try Camera.main as a fallback.
            if (interactionSource == null && Camera.main != null)
            {
                interactionSource = Camera.main.transform;
            }

            if (interactionSource == null)
            {
                Debug.LogWarning($"{nameof(interactor)}: interactionSource not set and no camera found. Assign a source in inspector.");
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (interactionSource == null)
                {
                    Debug.LogWarning($"{nameof(interactor)}: Cannot interact because interactionSource is null.");
                    return;
                }

                Debug.Log("Attempting interaction");
                Ray ray = new Ray(interactionSource.position, interactionSource.forward);

                // Visualize the ray for debugging (short-lived)
                Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.green, 1f);

                if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange, interactionMask))
                {
                    GameObject hitObj = hitInfo.collider.gameObject;
                    Debug.Log("Hit " + hitObj.name);

                    // Unity's TryGetComponent<T>() generic variant requires T : Component and can't be used with interfaces.
                    // Instead, search through MonoBehaviour components and test for the interface.
                    MonoBehaviour[] behaviours = hitObj.GetComponents<MonoBehaviour>();
                    for (int i = 0; i < behaviours.Length; i++)
                    {
                        if (behaviours[i] is IInteractible interactObj)
                        {
                            Debug.Log("Interacting with " + hitObj.name);
                            interactObj.Interact();
                            return;
                        }
                    }

                    Debug.Log("No IInteractible implementation found on " + hitObj.name);
                }
                else
                {
                    Debug.Log("No hit detected within range.");
                }
            }
        }
    }
