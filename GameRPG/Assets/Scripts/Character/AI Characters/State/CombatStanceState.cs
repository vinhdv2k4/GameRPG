using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
namespace TV
{
    [CreateAssetMenu(menuName = "A.I/States/Combo Stance ")]
    public class CombatStanceState : AiState
    {
        [Header("Attacks")]
        public List<AiCharacterAttackAction> aiCharacterAttacks;
        public List<AiCharacterAttackAction> potentialAttacks;
        private AiCharacterAttackAction choosenAttack;
        private AiCharacterAttackAction previousAttack;
        protected bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false;
        [SerializeField] protected int chanceToPerformCombo = 25;
        protected bool hasRolledComboChance = false;

        [Header("Engagement Distance")]
        [SerializeField] public float maximumEngagementDistance = 5;
        public override AiState Tick(CharacterAIManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this;
            if(!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;
            if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value) {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                {
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }
            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }
            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attack.currentAttack = choosenAttack;
                return SwitchState(aiCharacter, aiCharacter.attack);    
            }

            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance) 
                return SwitchState(aiCharacter, aiCharacter.persuaTarget);

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);
            return this;
        }

        protected virtual void GetNewAttack(CharacterAIManager aiCharacter)
        {
            potentialAttacks = new List<AiCharacterAttackAction>();
            foreach (var potentialAttack in aiCharacterAttacks)
            {
                if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;
                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;
                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;
                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;
                potentialAttacks.Add(potentialAttack);
            }
            if (potentialAttacks.Count <= 0)
                return;

            var totalWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            var randomWeightValue = Random.Range(1, totalWeight+1);
            var processWeight = 0;
            foreach (var attack in potentialAttacks)
            {
                processWeight += attack.attackWeight;
                if (randomWeightValue <= processWeight)
                {
                    choosenAttack = attack;
                    previousAttack = choosenAttack;
                    hasAttack = true;
                    return;
                }
            }

        }

        protected virtual bool RollForOutcomeChane(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;
            int randomPercentage = Random.Range(0, 100);
            if(randomPercentage< outcomeChance)
            {
                outcomeWillBePerformed = true;

            }
            return outcomeWillBePerformed;
        }

   

        protected override void ResetStateFlags(CharacterAIManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);
            hasAttack = false;
            hasRolledComboChance = false;
           
        }
    }
}
