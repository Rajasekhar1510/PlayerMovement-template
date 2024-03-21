using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rajasekhar
{
    public class InputManager : MonoBehaviour
    {
        PlayerControls playerControls; //reference to the input map action asset.
        AnimatorManager animatorManager;
        PlayerLocomotion playerLocomotion;

        [Header("Movement Values")]
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;
        [SerializeField] Vector2 movementInput; //up down & left right directions.

        [Header("CameraInput Values")]
        public float cameraInputX;
        public float cameraInputY;
        [SerializeField] Vector2 cameraInput; //up down & left right directions for camera.

        [Header("Button Input Booleans")]
        public bool sprintInput;
        public bool jumpInput = false;

        private void Awake()
        {
            animatorManager = GetComponent<AnimatorManager>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

                //we will add more adtions like camera and attack actions
                //Example
                //playerControls.PlayerAttack.LightAttack.performed += isPressed = true;

                playerControls.PlayerMovement.Sprint.performed += i => sprintInput = true;

                playerControls.PlayerMovement.Sprint.canceled += i => sprintInput = false;

                playerControls.PlayerMovement.Jump.performed += i => jumpInput = true;

                playerControls.PlayerMovement.Jump.canceled += i => jumpInput = false;

                playerControls.PlayerCamera.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        public void HandleALLInput()
        {
            //will be called in player manager's update function.
            HandleMovementInput();
            HandleJumpInput();
            HandleCameraInput();
            HandleSprintInput();
        }

        void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            //returns positive value always
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
            animatorManager.updateAnimatorValues(0, moveAmount, playerLocomotion.isSprint);
        }

        void HandleCameraInput()
        {
            cameraInputY = cameraInput.y;
            cameraInputX = cameraInput.x;
        }

        void HandleSprintInput()
        {
            if(sprintInput && moveAmount > 0.5f)
            {
                playerLocomotion.isSprint = true;
            }
            else
            {
                playerLocomotion.isSprint = false;
            }
        }

        void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;
                playerLocomotion.HandleJump();
            }
        }

    }

}
