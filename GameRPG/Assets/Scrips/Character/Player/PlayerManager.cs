using UnityEngine;
namespace TV {
    public class PlayerManager : CharacterManager
    {
       [HideInInspector]public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        protected override void Awake()
        {
            
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }
        protected override void Update()
        {
            
            base.Update();
            // neu khong phai la owner thi khong cho di chuyen
            if (!IsOwner)
                return;
            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenarateStamina();

        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;

            base.LateUpdate();
           
            PlayerCamera.instance.HandleAllCameraActions();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManger.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                playerNetworkManager.maxStamina.Value = playerStatsManager.CaculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatsManager.CaculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                PlayerUIManger.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
                

            }
        }

        public void SaveGameCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.zPosition = transform.position.z;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData) {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition,currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;

        }
    }
}
