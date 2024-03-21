using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rajasekhar
{
    public class PlayerManager : MonoBehaviour, IDataPersistence
    {
        [Header("REFERENCES")]
        InputManager inputManager;
        PlayerLocomotion playerLocomotion;
        CameraManager cameraManager;
        Animator animator;
        public bool isInteracting;

        private void Awake()
        {
            cameraManager = FindObjectOfType<CameraManager>();
            inputManager = GetComponent<InputManager>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animator = GetComponent<Animator>();

        }

        private void Update()
        {
            inputManager.HandleALLInput();
        }

        private void FixedUpdate()
        {
            playerLocomotion.HandleALLMovement();
        }

        private void LateUpdate()
        {
            cameraManager.HandleALLCameraMovement();

            //below is the boolean for is intereacting from the animator manager script that allowas to play fall and land animations
            isInteracting = animator.GetBool("isInteracting");
            //below is the jump boolean initialization
            playerLocomotion.isJumping = animator.GetBool("isJumping");
            animator.SetBool("isGrounded", playerLocomotion.isGrounded);
        }

        //LOADING AND SAVING THE PLAYERS POSTION---------->>>>>>
        #region LOADING SAVING SAVING PLAYER POS
        public void LoadData(GameData gameData)
        {
            Vector3 myPos = new Vector3(gameData.xPosition, gameData.yPosition, gameData.zPosition);
            gameObject.transform.position = myPos;
            Physics.SyncTransforms();
            Debug.Log("Player pos:" + myPos);
        }
        public void SaveData(ref GameData gameData)
        {
            gameData.xPosition = this.transform.position.x;
            gameData.yPosition = this.transform.position.y;
            gameData.zPosition = this.transform.position.z;
            
        }
        #endregion

    }
}

