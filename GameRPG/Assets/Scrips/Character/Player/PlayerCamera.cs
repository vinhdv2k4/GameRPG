using UnityEngine;

namespace TV
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform; // Điểm pivot để xoay camera

        [Header("Camera Settings")]
        [SerializeField] private float cameraSmoothTime = 0.1f;
        [SerializeField] private float leftAndRightSpeed = 220f; // Tốc độ xoay ngang
        [SerializeField] private float upAndDownSpeed = 220f;   // Tốc độ xoay dọc
        [SerializeField] private float minimumPivot = -30f;
        [SerializeField] private float maximumPivot = 60f;
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] private LayerMask collideWithLayers;
        [SerializeField] private float cameraDistance = 5f; // Khoảng cách từ camera đến nhân vật

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private float leftAndRightLookAngle;
        private float upAndDownLookAngle;
        private float cameraZPosition; // VALUE USED FOR THE CAMERA COLLISIONS
        private float targetCameraZPosition; // VALUE USED FOR THE CAMERA COLLISIONS
        private Vector3 cameraObjectPosition; 

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player != null && PlayerInputManager.instance != null)
            {
                HandleRotation();
                HandleFollowTarget();
                HandleCollisions();
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothTime * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * leftAndRightSpeed * Time.deltaTime;
            upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * upAndDownSpeed * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation;
            Quaternion targetRotation;
            // Rotate gameobject left and right
            cameraRotation = Vector3.zero;
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // rotate up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }



        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            // DIRECTION FOR COLLISION CHECK
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            //  CHECK IF THERE IS AN OBJECT IN FRONT OF OUR DESIRED DIRECTION * (SEE ABOVE FOR DIRECTION)
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                // IF THERE IS, WE GET OUR DISTANCE FROM IT
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // WE THEN EQUATE OUR TARGET Z POSITION TO THE FOLLOWING 
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            // IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS (MAKING IT SNAP BACK)
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;

        }
    }
}