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


            instance.enabled = false;
            SceneManager.activeSceneChanged += OnSceneChanged;


        }
        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            // neu loa den scene moi thi bat input
            if (newScene.buildIndex == WorldGameSave.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }
        private void OnEnable()
        {
            if (playerControll == null)
            {
                playerControll = new PlayerControlls();
                playerControll.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();
                playerControll.PlayerMovement.Movement.canceled += i => movement = Vector2.zero;
                playerControll.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControll.PlayerCamera.Movement.canceled += i => cameraInput = Vector2.zero;
                playerControll.PlayerActions.Dodge.performed += i => dodgeInput = true;
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
            player.playerAnimatorManager.UpadateAnimatorMovementParameters(0,moveAmount);

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
                player.playerLocomotionManager.AttemptToPerformmDodge();
                 
            }
        }
    }
}