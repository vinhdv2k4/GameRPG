using Unity.VisualScripting;
using UnityEngine;
namespace TV
{
    public class PlayerLocomotionManager : CharaterLocomotionManager
    {
        public PlayerManager player;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Setting")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkSpeed = 2f;
        [SerializeField] float runSpeed = 5f;
        [SerializeField] float sprintSpeed = 7f;
        [SerializeField] float speedRotation = 15f;
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Dodge Setting")]
        private Vector3 rollDirection;
        private float dodgeCost = 25f;

        protected override void Awake()
        {

            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        protected override void Update()
        {
            base.Update();
            // neu khong phai la owner thi khong cho di chuyen
            if (player.IsOwner)
            {
                player.characterNetworkManager.networkAnimatorVertical.Value = verticalMovement;
                player.characterNetworkManager.networkAnimatorHorizontal.Value = horizontalMovement;
                player.characterNetworkManager.networkAnimatorMoveAmout.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.networkAnimatorVertical.Value;
                horizontalMovement = player.characterNetworkManager.networkAnimatorHorizontal.Value;
                moveAmount = player.characterNetworkManager.networkAnimatorMoveAmout.Value;

                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            }

        }
        public void HandleAllMovement()
        {
         
            HandleGroundedMovement();
            HandleRotation();
        }
        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }

        private void HandleGroundedMovement()
        {
            if(!player.canMove)
                return;
            GetMovementValues();
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintSpeed*Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5)
                {
                    player.characterController.Move(moveDirection * runSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5)
                {
                    player.characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
                }

            }
         }
        private void HandleRotation()
        {
            if (!player.canRotate)
                return;
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;
            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, speedRotation * Time.deltaTime);
            transform.rotation = targetRotation;

        }
        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }
            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

           

        public void AttemptToPerformmDodge()
        {
            if (player.isPerformingAction)
                return;
            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;
            // if moving 
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

                rollDirection.y = 0;
                rollDirection.Normalize();
                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayerTargetActionAnimation("Roll_Forward_01", true,true);

                // perform a roll animation

            }
            else
            {
                // perform a backstep animation
                player.playerAnimatorManager.PlayerTargetActionAnimation("Back_Step_01", true, true);
            }
            player.playerNetworkManager.currentStamina.Value -= dodgeCost;
        }
    }
}
