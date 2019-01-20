using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PKClone
{

    public class PlayerCamera : MonoBehaviour
    {
        public bool LockCursor = true;

        public float MouseSensitivity = 2;

        public Transform Target;
        public float MaxDistanceFromTarget = 2;

        public Vector2 PitchMinMax = new Vector2(-80, 85);
        public bool InversePitch;
        public bool InverseYaw;

        public float SmoothTime = 0.12f;

        private Vector3 rotationSmoothVelocity;
        private Vector3 currentRotation;

        private Vector3 desiredRotation = Vector3.zero;

        public void Start()
        {
            if (LockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                // Cursor.visible = false;
            }
        }

        public void LateUpdate()
        {
            Vector3 inputFrame = new Vector3(Input.GetAxis("Mouse Y") * MouseSensitivity * (InverseYaw ? -1 : 1),
                Input.GetAxis("Mouse X") * MouseSensitivity * (InversePitch ? -1 : 1), 0);
            desiredRotation += inputFrame;

            desiredRotation.x = Mathf.Clamp(desiredRotation.x, PitchMinMax.x, PitchMinMax.y);

            currentRotation =
                Vector3.SmoothDamp(currentRotation, desiredRotation, ref rotationSmoothVelocity, SmoothTime);
            transform.eulerAngles = currentRotation;

            float distanceFromTarget = MaxDistanceFromTarget;
            RaycastHit hitInfo;
            if (Physics.Raycast(new Ray(Target.position, -transform.forward), out hitInfo))
            {
                if (hitInfo.distance < MaxDistanceFromTarget)
                {
                    distanceFromTarget = hitInfo.distance - 0.001f;
                }
            }

            transform.position = Target.position - transform.forward * distanceFromTarget;
        }
    }
}