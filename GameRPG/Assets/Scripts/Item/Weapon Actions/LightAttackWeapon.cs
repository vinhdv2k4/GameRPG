using TV;
using UnityEngine;
namespace TV
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ Light Attack Actions")]
    public class LightAttackWeapon : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
        [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";
        public override void AttemptToPerformAcrion(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAcrion(playerPerformingAction, weaponPerformingAction);
            if (!playerPerformingAction.IsOwner)
                return;
            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;
            if(!playerPerformingAction.characterLocomotionManager.isGrounded)
                return;
            PerformingLightAttack(playerPerformingAction, weaponPerformingAction);  

        }

        private void PerformingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
           if(playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackStyle.lightAttack02, light_Attack_02, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackStyle.lightAttack01, light_Attack_01, true);
                }
            }
            else if(!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackStyle.lightAttack01, light_Attack_01, true);
            }
            
        }
    }
}
