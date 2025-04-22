using UnityEngine;
namespace TV
{
    public class PlayerLocomotionManager : CharaterLocomotionManager
    {
        public PlayerManager player;
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkSpeed = 2f;
        [SerializeField] float runSpeed = 5f;
        [SerializeField] float speedRotation = 15f;
        protected override void Awake()
        {
           
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }
        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
        }

        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (PlayerInputManager.instance.moveAmount > 0.5)
            {
                player.characterController.Move(moveDirection * runSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5)
            {
                player.characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
            }
        }
        private void HandleRotation()
        {
             targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;
            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection =transform.forward;
            }
           Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation= Quaternion.Slerp(transform.rotation, newRotation,speedRotation* Time.deltaTime);
            transform.rotation = targetRotation;

        }
    }
}
