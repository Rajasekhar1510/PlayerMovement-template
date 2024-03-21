using Rajasekhar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rajasekhar
{
    public class CameraManager : MonoBehaviour
    {
        InputManager inputManager;

        [Header("Camera Sensitivity & Follow values")]
        public float cameraLookSpeed = 2;
        public float cameraPivotSpeed = 2;
        public float cameraFollowSpeed = 0.2f;

        [Header("Camera Values")]
         float cameraCollisionRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f; //how much camera will jump off the objects.
        public float minCollisionOffset = 0.2f;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        [Header("Look Angles")]
        [SerializeField] float pivotAngle; //look left and right
        [SerializeField] float lookAngle; //look up and down
        public float minPivotAngle = -35;
        public float maxPivotAngle = 35;

        [Header("Transforms")]
        public Transform cameraTransform; //transform of actaul camera obj.
        public Transform targetTransform; //the obj the camera will follow
        public Transform cameraPivot; //the obj camera uses to look up and down
        private float defaultPosition;
        private Vector3 cameraVectorPos;

        [Header("Layer Options")]
        public LayerMask collisionLayers; //the layers camera will collide with

        private void Awake()
        {
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            cameraTransform = Camera.main.transform;
            defaultPosition = cameraTransform.localPosition.z;
            inputManager = FindObjectOfType<InputManager>();
        }

        public void HandleALLCameraMovement()
        {
            FollowTarget();
            RotateCamera();
            HandleCollisions();
        }

        #region Camera following the target
        private void FollowTarget()
        {
            Vector3 targetPosition = Vector3.SmoothDamp
                (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

            transform.position = targetPosition;
        }
        #endregion

        #region Rotating camera
        private void RotateCamera()
        {
            Vector3 rotation;
            Quaternion targetRot;

            lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
            pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);

            //to limit the up and down movement of the camera, we clamp the positions of the pivot angle:
            pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

            rotation = Vector3.zero;
            rotation.y = lookAngle;
            targetRot = Quaternion.Euler(rotation);
            transform.rotation = targetRot;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRot = Quaternion.Euler(rotation);
            cameraPivot.localRotation = targetRot;
        }
        #endregion

        #region camera collisions
        private void HandleCollisions()
        {
            float targetPos = defaultPosition;
            RaycastHit hit;

            Vector3 dir = cameraTransform.position - cameraPivot.position;
            dir.Normalize();

            if (Physics.SphereCast
                (cameraPivot.transform.position, cameraCollisionRadius, dir, out hit, Mathf.Abs(targetPos), collisionLayers))
            {
                float distance = Vector3.Distance(cameraPivot.position, hit.point);
                targetPos =- (distance - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPos) < minCollisionOffset)
            {
                targetPos = targetPos - minCollisionOffset;
            }

            cameraVectorPos.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPos, 0.2f);
            cameraTransform.localPosition = cameraVectorPos;

        }

        #endregion

    }
}

