using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
namespace TV

{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;
        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>(new FixedString64Bytes("Character"), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
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


    }
}
