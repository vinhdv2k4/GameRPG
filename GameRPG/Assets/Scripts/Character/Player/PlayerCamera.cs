using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
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

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private float leftAndRightLookAngle;
        private float upAndDownLookAngle;
        private float cameraZPosition; // VALUE USED FOR THE CAMERA COLLISIONS
        private float targetCameraZPosition; // VALUE USED FOR THE CAMERA COLLISIONS
        private Vector3 cameraObjectPosition;

        [Header("Lock On")]
        [SerializeField]  float LockOnRadius = 50f; 
        [SerializeField]  float minimumViewAbleAngle = -50f; 
        [SerializeField]  float maximumViewAbleAngle = 50f;
        [SerializeField] float seCameraHeightSpeed= 1f; 
        [SerializeField] float lockOnFollowSpeed = 0.2f;
        [SerializeField] float unLockCameraHight =1.65f;
        [SerializeField] float lockOnCameraHeight = 2f;
        private Coroutine cameraLockOnHeightCoroutine;
        private List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockOnTarget;
        public CharacterManager rightLockOnTarget;
       


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
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnFollowSpeed);

                rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnFollowSpeed);

                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.localEulerAngles.x;
            }
            else
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

        public void HandleLocatingLockOnTarget()    
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;
            float shortestDistanceOfLeftTarget =- Mathf.Infinity;

            Collider[] collliders = Physics.OverlapSphere(player.transform.position, LockOnRadius, WorldUnityManager.instance.GetCharacterLayers());
            for (int i=0; i< collliders.Length; i++)
            {
                CharacterManager lockOnTarget = collliders[i].GetComponent<CharacterManager>();
                if(lockOnTarget != null)
                {
                    Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                    if (lockOnTarget.isDead.Value)
                    {
                        continue;
                    }
                    if (lockOnTarget.transform.root == player.transform.root)
                        continue;

                    if(viewableAngle > minimumViewAbleAngle && viewableAngle < maximumViewAbleAngle)
                    {
                        RaycastHit hit;

                       if(Physics.Linecast(
                           player.playerCombatManager.lockOnTransform.position,
                           lockOnTarget.characterCombatManager.lockOnTransform.position,
                           out hit, 
                           WorldUnityManager.instance.GetEnvironmentLayers()))
                        {
                            continue;
                        }
                        else
                        {
                            availableTargets.Add(lockOnTarget);
                        }
                    }

                }
            }
            for (int k = 0; k < availableTargets.Count; k++)
            {
                if (availableTargets[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);

                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = availableTargets[k];
                    }
                    if (player.playerNetworkManager.isLockedOn.Value)
                    {
                        Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if (availableTargets[k] != player.playerCombatManager.currentTarget)
                            continue;

                        if(relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget>shortestDistanceOfLeftTarget)
                        {
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = availableTargets[k];
                        }
                        else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget > shortestDistanceOfRightTarget )
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = availableTargets[k];
                        }
                    }
                }
                else
                {
                    ClearLockOnTarget();
                    player.playerNetworkManager.isLockedOn.Value = false;
                }
            }
        }

        public void SetLockCameraHeight()
        {
            if(cameraLockOnHeightCoroutine != null)
            {
                StopCoroutine(cameraLockOnHeightCoroutine);
            }
            cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
        }

        public void ClearLockOnTarget()
        {
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;
            availableTargets.Clear();
        }

        public IEnumerator WaitThemFindNewTarget()
        {
            while (player.isPerformingAction)
            {
                yield return null;
            }
            ClearLockOnTarget();
            HandleLocatingLockOnTarget();
            if(nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }
            yield return null;
        }

        private IEnumerator SetCameraHeight()
        {
            float duration = 1;
            float timer = 0;
            
            Vector3 velocity = Vector3.zero;
            Vector3 newLockTargetHeight = new Vector3(cameraPivotTransform.transform.localPosition.x,lockOnCameraHeight);
            Vector3 newUnLockTargetHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unLockCameraHight);

            while (timer < duration)
            {
                timer += Time.deltaTime;
                if(player != null)
                {
                    if(player.playerCombatManager.currentTarget != null)
                    {
                        cameraPivotTransform.transform.localPosition = 
                            Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockTargetHeight, ref velocity, seCameraHeightSpeed);
                        cameraPivotTransform.transform.localRotation = 
                            Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0,0,0),lockOnFollowSpeed );
                    }
                    else
                    {
                        cameraPivotTransform.transform.localPosition =
                            Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockTargetHeight, ref velocity, seCameraHeightSpeed);
                    }
                }
               yield return null;
            }

            if(player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = newLockTargetHeight;
                    cameraPivotTransform.transform.localRotation =Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = newUnLockTargetHeight;
                }
            }
            yield return null;
        }
    }
}