using UnityEngine;
namespace TV
{
    public class AiCharacterLocomotionManager : CharacterLocomotionManager

    {
        public void RotateTowardsAgent(CharacterAIManager aicharacter)
        {
            if (aicharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aicharacter.transform.rotation =aicharacter.navMeshAgent.transform.rotation;
            }
        }
    }   
}   
