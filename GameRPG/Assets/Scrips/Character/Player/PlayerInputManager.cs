using UnityEngine;
using UnityEngine.SceneManagement;
namespace TV
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;


        PlayerControlls playerControll;

        [Header("Player Movement")]
        [SerializeField] Vector2 movement;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("Player Camera")]
        [SerializeField] public Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("Player Actions Input")]
        [SerializeField ] bool  dodgeInput =false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool RB_Input =false;
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
            SceneManager.activeSceneChanged += OnSceneChanged;


            instance.enabled = false;

            if(playerControll != null)
            {
                playerControll.Disable();
            }

        }
        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            // neu loa den scene moi thi bat input
            if (newScene.buildIndex == WorldGameSave.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
                if(playerControll != null)
                {
                    playerControll.Enable();
                }
            }
            else
            {
                instance.enabled = false;

                if (playerControll != null)
                {
                    playerControll.Disable();
                }
            }
        }
        private void OnEnable()
        {
            if (playerControll == null)
            {
                playerControll = new PlayerControlls();
                playerControll.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();

                playerControll.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControll.PlayerCamera.Movement.canceled += i => cameraInput = Vector2.zero;

                playerControll.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControll.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControll.PlayerActions.RB.performed += i => RB_Input = true;

                // hold input , set bool =true and release set bool = false
                playerControll.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControll.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }
            playerControll.Enable();

        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
        // neu khong o trong man hinh thi dung lay du lieu input
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControll.Enable();
                }
                else
                {
                    playerControll.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();

        }
        private void HandleAllInputs()
        {
            HandeleCameraMovement();
            HandlePlayerMovement();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();

        } 
        private void HandlePlayerMovement()
        {
            verticalInput = movement.y;
            horizontalInput = movement.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
            if (moveAmount < 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount >= 0.5f && moveAmount <= 1)
            {
                moveAmount = 1f;
            }
            if (player == null)
                return;
            // if are not locked on, only use move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

        }
        
        private void HandeleCameraMovement()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;
                player.playerLocomotionManager.AttemptToPerformDodge();
                 
            }
        }
        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
               player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

       private void HandleRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;
                player.playerNetworkManager.SetCharacterActionHand(true);

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightWeapon.oh_RB_Action, player.playerInventoryManager.currentRightWeapon);
            }
        }
    }
}