using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;
namespace TV

{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;
        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>(new FixedString64Bytes("Character"), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equiment")]
        public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public void SetCharacterActionHand(bool righthandAction)
        {
            if (righthandAction)
            {
                isUsingLeftHand.Value = false;
                isUsingRightHand.Value = true;
            }
            else
            {
                isUsingLeftHand.Value = true;
                isUsingRightHand.Value = false;
            }
        }
        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = player.playerStatsManager.CaculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManger.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }
        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = player.playerStatsManager.CaculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManger.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDataBase.instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentRightWeapon = newWeapon;
            player.playerEquitmentManager.LoadRightWeapon();
        }

        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {

            WeaponItem newWeapon = Instantiate(WorldItemDataBase.instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentLeftWeapon = newWeapon;
            player.playerEquitmentManager.LoadLeftWeapon();
        }


        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newId)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDataBase.instance.GetWeaponByID(newId));

        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;
           
        }

        [ServerRpc]
        public void NotifyTheServerWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
        {
            if (IsServer)
            {
                NotifyServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
            }
        }

        [ClientRpc]
        public void NotifyServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
        {
            if(clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }

        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weapomAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

            if(weapomAction != null)
            {
                weapomAction.AttemptToPerformAcrion(player,WorldItemDataBase.instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("Weapon action with ID not found.");
            }
        }
       



    }
}
