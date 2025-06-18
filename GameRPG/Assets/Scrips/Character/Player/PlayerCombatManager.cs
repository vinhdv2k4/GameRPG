using Unity.Netcode;
using UnityEngine;
namespace TV
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            weaponAction.AttemptToPerformAcrion(player, weaponPerformingAction);

            player.playerNetworkManager.NotifyTheServerWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);

        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            if(!player.IsOwner)
                return;

            if (currentWeaponBeingUsed == null)
                return;
            float staminaDecducted = 0;

            switch (currentAttackSyle)
            {
                case AttackStyle.lightAttack:
                    staminaDecducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }

            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDecducted);
        }
        

    }
}
