using TV;
using UnityEngine;
namespace TV
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/ Light Attack Actions")]
    public class LightAttackWeapon : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
        public override void AttemptToPerformAcrion(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAcrion(playerPerformingAction, weaponPerformingAction);
            if (!playerPerformingAction.IsOwner)
                return;
            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;
            if(!playerPerformingAction.isGrounded)
                return;
            PerformingLightAttack(playerPerformingAction, weaponPerformingAction);  

        }

        private void PerformingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
           
            if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackStyle.lightAttack,light_Attack_01,true);
            }
            if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
            {
            }
        }
    }
}
