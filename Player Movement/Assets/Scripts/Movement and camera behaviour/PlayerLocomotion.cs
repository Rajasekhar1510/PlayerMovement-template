using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rajasekhar
{
    public class PlayerLocomotion : MonoBehaviour
    { 
        PlayerManager playerManager;
        InputManager inputManager;
        Vector3 moveDirection;
        Transform cameraObject;
        Rigidbody playerRB;
        AnimatorManager animationManager;

        [Header("falling")]
        public float inAirTimer;
        public float leapingVelocity;
        public float fallingVelocity;
        public LayerMask groundLayer;
        public float rayCastHeightOffSet = 0.1f;

        [Header("Jumping")]
        public float gravityIntensity = -15; //This variable must always be negative or else it will go inground and not jump
        public float jumpHeight = 3;

        [Header("Movement Flags")]
        public bool isSprint;
        public bool isGrounded;
        public bool isJumping;

        [Header("Movement Speeds")]
        public float walkingSpeed = 1.5f;
        public float runningSpeed = 5;
        public float sprintingSpeed = 7;
        public float rotationSpeed = 15;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            playerRB = GetComponent<Rigidbody>();
            cameraObject = Camera.main.transform;
            playerManager = GetComponent<PlayerManager>();
            animationManager = GetComponent<AnimatorManager>();
            isGrounded = true;
        }

        public void HandleALLMovement()
        {
            HandleFallAndLand();

            if (!playerManager.isInteracting)
            {
                return;
            }

            HandleMovement();
            HandleRotation();
        }

        #region Handling the player Movement
        private void HandleMovement()
        {
            if (isJumping)
            { return; }

            moveDirection = cameraObject.forward * inputManager.verticalInput;
            moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
            moveDirection.Normalize();
            moveDirection.y = 0;
            //If we are sprinting, select the sprint speed. If the joystick is halfway pressed, use walk speed and vice versa.

            if (isSprint)
            {
                //sprinting action
                moveDirection *=  sprintingSpeed;
            }
            else
            {
                if (inputManager.moveAmount >= 0.5f)
                {
                    moveDirection *= runningSpeed;
                }
                else
                {
                    moveDirection *= walkingSpeed; //direction * speed
                }
            }

            Vector3 movementVelocity = moveDirection;
            playerRB.velocity = movementVelocity;//moves the player rigid body based on the above calcs.
        }
        #endregion

        #region Handling the rotation of the player respective to the camera
        private void HandleRotation()
        {
            if(isJumping)
            { return; }
            Vector3 targetDir = Vector3.zero;
            targetDir = cameraObject.forward * inputManager.verticalInput;
            targetDir = targetDir + cameraObject.right *inputManager.horizontalInput;
            targetDir.Normalize();
            targetDir.y = 0;

            if(targetDir == Vector3.zero)
                targetDir = transform.forward;

            Quaternion targetRot = Quaternion.LookRotation(targetDir); //looking towards our target direction
            Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime); //move from current rotation to the target rotation.

            transform.rotation = playerRot; //set the transform rotation to the player rotataion
        }

        #endregion

        #region falling and landing
        /* 
         How this basically works:
        -When the player is NOT Grounded and is INAIR, the isIntereacting bool in PLM is set to false.
        -if the isInteracting bool in PLM is flase, we play the falling animation. 
        -The in air time variable will be multiplied by time.
        -The rigid body on the player and it's transform will be multiplied by a leaping velocity 
         which will inturn push the player with a certain force so the player can fall down when the player is off the ledge.
        -while the player is falling, we add a downward force by taking the downward vector of the rigidbody and multiplying it with fallingVelocity and inAirTimer
        -where in this case, the inAirTimer is multiplied by time and falling velocity is he amount at which we want the player to fall to.
        -in other words, how fast the player will fall.
         
        To check weather the player is grounded, we cast a Spherecast to the downward direction of the player
        it checks if the ray casted from the sphereCast is hitting the Ground layer or not. 
        If it isn't, the player is not grounded and we play the Land animation
        The InAir timer is set to 0 and the player will be grounded, the isIntereacting bool of PLM will be true.
        Else, the the player will be not grounded which will inturn start the if statement of the player not being grounded.
         */
        void HandleFallAndLand()
        {
            RaycastHit hit;
            Vector3 rayCastOrigin = transform.position;
            rayCastOrigin.y += rayCastHeightOffSet;
            Vector3 targetPos;
            targetPos = transform.position;
            // Debug.DrawLine(rayCastOrigin, rayCastOrigin - Vector3.up * maxDistance, Color.red);
            if (Physics.SphereCast(rayCastOrigin, 0.3f, -Vector3.up, out hit, groundLayer))
            {
                if (!isGrounded && !playerManager.isInteracting)
                {
                    animationManager.PlayTargetAnimation("Land", true);
                }

                Vector3 rayCastHitPoint = hit.point;
                targetPos.y = rayCastHitPoint.y;
                inAirTimer = 0;
                isGrounded = true;
                playerManager.isInteracting = true;

            }
            else
            {
                isGrounded = false;

            }
            if (isGrounded && !isJumping)
            {
                if (playerManager.isInteracting || inputManager.moveAmount > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPos;
                }
            }

            if (!isGrounded)
            {
                playerManager.isInteracting = false;

                if (!playerManager.isInteracting)
                {
                    animationManager.PlayTargetAnimation("Falling", true);
                    
                }

                inAirTimer += Time.fixedDeltaTime;
                playerRB.AddForce(transform.forward * leapingVelocity);
                playerRB.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
            }


        }
        #endregion

        #region JUMPING
        public void HandleJump()
        {
            if (isJumping)
            {
                return;
            }

            if(isGrounded)
            {
                animationManager.animator.SetBool("isJumping", true);
                animationManager.PlayTargetAnimation("Jump", false);

                float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);  
                Vector3 playerVelocity = moveDirection;
                playerVelocity.y = jumpingVelocity;
                playerRB.velocity = playerVelocity;
                return;

            }
        }
        #endregion


    }

}
