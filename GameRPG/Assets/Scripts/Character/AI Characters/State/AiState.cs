using UnityEngine;
namespace TV
{
    public class AiState : ScriptableObject
    {
        public virtual AiState Tick(CharacterAIManager characterAIManager)
        {

            return this;
        }

        protected virtual AiState SwitchState(CharacterAIManager aiCharacter, AiState newState)
        {
            ResetStateFlags(aiCharacter);
            return newState;
        }

        protected virtual void ResetStateFlags(CharacterAIManager aiCharacter)
        {
            
        }
    }
}
