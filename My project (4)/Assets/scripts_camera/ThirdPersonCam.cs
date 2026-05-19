using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.scripts_camera
{
    public class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        public Transform orientation;
        public Transform player;
        public Transform playerObject;
        public Transform objectHolder;
        public Rigidbody rb;

        [Header("Settings")]
        public float rotationSpeed = 50f;

        [Tooltip("Toggle to invert vertical mouse movement")]
        public bool invertY = false;

        public CameraStyle camStyle;
        public Transform combatLookAt;

        [Header("Combat Zone Control")]
        [Tooltip("If true, camera is forced into combat mode by a BattleZone")]
        public bool combatLocked = false;

        public enum CameraStyle
        {
            Basic,
            Combat
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            camStyle = CameraStyle.Basic;
        }

        private void Update()
        {
            HandleCameraMode();
            HandleOrientation();
            HandlePlayerRotation();
            HandleMouseRotation();
        }

        void HandleCameraMode()
        {
            // Only allow manual combat toggle if not locked by a zone
            if (!combatLocked)
            {
                if (Input.GetMouseButtonDown((int)MouseButton.Right))
                {
                    camStyle = CameraStyle.Combat;
                }
                else if (Input.GetMouseButtonUp((int)MouseButton.Right))
                {
                    camStyle = CameraStyle.Basic;
                }
            }
        }

        void HandleOrientation()
        {
            Vector3 viewDirection = player.position -
                new Vector3(transform.position.x, player.position.y, transform.position.z);

            orientation.forward = viewDirection.normalized;
        }

        void HandlePlayerRotation()
        {
            if (camStyle == CameraStyle.Basic)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                Vector3 inputDir =
                    orientation.forward * verticalInput +
                    orientation.right * horizontalInput;

                if (inputDir != Vector3.zero)
                {
                    playerObject.forward = Vector3.Slerp(
                        playerObject.forward,
                        inputDir.normalized,
                        Time.deltaTime * rotationSpeed
                    );
                }
            }
            else if (camStyle == CameraStyle.Combat)
            {
                if (combatLookAt == null) return;

                Vector3 dirToCombatLookAt =
                    combatLookAt.position -
                    new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);

                orientation.forward = dirToCombatLookAt.normalized;
                playerObject.forward = dirToCombatLookAt.normalized;
            }
        }

        void HandleMouseRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Horizontal rotation
            objectHolder.Rotate(Vector3.up * mouseX, Space.World);

            // Vertical rotation
            float yRotation = invertY ? -mouseY : mouseY;
            objectHolder.Rotate(Vector3.right * yRotation, Space.Self);
        }
    }
}