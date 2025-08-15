using UnityEngine;
namespace TV
{
    [CreateAssetMenu(menuName ="A.I/States/Idle")]
    public class IdleState : AiState
    {
        public override AiState Tick(CharacterAIManager aiCharacter)
        {
            if(aiCharacter.characterCombatManager.currentTarget!= null)
            {
                return SwitchState(aiCharacter, aiCharacter.persuaTarget);
            }
            else
            {
              aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
                return this;
            }
           
        }
    }
}
