using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PKClone
{

    public class PlayerController : MonoBehaviour
    {
        public float Speed = 5;

        public float Gravity = -10;

        public float SmallJumpGravityMultiplier = 2;
        private bool isJumping = false;

        public float JumpHeight = 2;

        [Range(0, 1)] public float AirControlPercent;

        public float TurnSmoothTime = 0.2f;
        private float turnSmoothVelocity;

        public float SpeedSmoothTime = 0.1f;
        private float speedSmoothVelocity;

        private float currentSpeed;
        private float velocityY;

        public Transform Camera;
        private CharacterController controller;

        public void Start()
        {
            this.controller = GetComponent<CharacterController>();
        }

        public void Update()
        {
            if (controller.isGrounded)
            {
                isJumping = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 inputDir = input.normalized;

            Move(inputDir);
        }

        private void Jump()
        {
            if (controller.isGrounded)
            {
                float jumpVelocity = Mathf.Sqrt(-2 * Gravity * JumpHeight);
                velocityY = jumpVelocity;
                isJumping = true;
            }
        }

        private void Move(Vector2 inputDir)
        {
            if (inputDir != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + Camera.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                                            ref turnSmoothVelocity, CalculateSmoothTimeByState(TurnSmoothTime));
            }

            float targetSpeed = Speed * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity,
                CalculateSmoothTimeByState(SpeedSmoothTime));

            float currentGravity = Gravity;

            if (isJumping && !Input.GetKey(KeyCode.Space))
            {
                currentGravity *= SmallJumpGravityMultiplier;
            }

            velocityY += Time.deltaTime * currentGravity;
            Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);
            currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

            if (controller.isGrounded)
            {
                velocityY = 0;
            }
        }

        private float CalculateSmoothTimeByState(float smoothTime)
        {
            if (controller.isGrounded)
                return smoothTime;

            if (AirControlPercent == 0)
            {
                return float.MaxValue;
            }

            return smoothTime / AirControlPercent;
        }
    }
}