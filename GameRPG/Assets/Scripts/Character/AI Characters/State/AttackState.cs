using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TV
{
    [CreateAssetMenu(menuName = "A.I/States/Attack")]
    public class AttackState : AiState
    {
        [Header("Current Attack")]
        [HideInInspector] public AiCharacterAttackAction currentAttack;
        [HideInInspector] public bool willPerformCombo=false;

        [Header("State Flags")]
        protected bool hasPerformedAttack = false;
        protected bool hasPerformedCombo = false;

        [Header("Pivot After Attack")]
        [SerializeField] protected bool pivotAfterAttack = true;

        public override AiState Tick(CharacterAIManager aiCharacter)
        {
          if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

          if(aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
                return SwitchState(aiCharacter, aiCharacter.idle);

            aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhilstAttacking(aiCharacter);

            aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

          if(willPerformCombo && !hasPerformedCombo)
            {
                if(currentAttack.comboAction != null)
                {
                    //hasPerformedCombo = true;
                    //currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
                }
            }


            if (aiCharacter.isPerformingAction)
                return this;
            if (!hasPerformedAttack)
            {
                if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                    return this;
                PerformAttack(aiCharacter);
                return this;
            }

            if (pivotAfterAttack)
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);

            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }
        protected void PerformAttack(CharacterAIManager aiCharacter)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction(aiCharacter);
            aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
        }
        protected override void ResetStateFlags(CharacterAIManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);
            hasPerformedAttack = false;
            hasPerformedCombo = false;
        }
    }
}