using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TV {
    public class PlayerManager : CharacterManager
    {
        [Header("DEBUG MENU")]
        [SerializeField] bool respawnCharacter = false;
       [HideInInspector]public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;

        protected override void Awake()
        {
            
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
        }
        protected override void Update()
        {
            
            base.Update();
            // neu khong phai la owner thi khong cho di chuyen
            if (!IsOwner)
                return;
            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenarateStamina();
            DebugMenu();

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
                WorldGameSave.instance.player = this;

                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;


                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManger.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManger.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

            }
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                PlayerUIManger.instance.playerUiPopUpManager.SendYouDiePopUp();
            }
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
            
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();
            if (IsOwner)
            {
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

                playerAnimatorManager.PlayerTargetActionAnimation("Empty", false);
            }
        }

        public void SaveGameCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;

            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.curremtStamina = playerNetworkManager.currentStamina.Value;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData) {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition,currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;

            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;

            playerNetworkManager.maxHealth.Value = playerStatsManager.CaculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CaculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value = currentCharacterData.curremtStamina;
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            PlayerUIManger.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

        }

        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }
        }
    }
}
