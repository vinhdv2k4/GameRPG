using UnityEngine;
namespace TV
{
    public class AiCharacterAnimatorManager : CharacterAnimatorManager
    {
        CharacterAIManager aiCharacter;
        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<CharacterAIManager>();
        }

        private void OnAnimatorMove()
        {
            //Host
            if (aiCharacter.IsOwner)
            {
                if (!aiCharacter.characterLocomotionManager.isGrounded)
                    return;
                Vector3 velocity = aiCharacter.animator.deltaPosition;

                aiCharacter.characterController.Move(velocity );
                aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
            }
            else
            {
                if (!aiCharacter.characterLocomotionManager.isGrounded)
                    return;
                Vector3 velocity = aiCharacter.animator.deltaPosition;

                aiCharacter.characterController.Move(velocity);
                aiCharacter.transform.position = Vector3.SmoothDamp(transform.position,
                    aiCharacter.characterNetworkManager.networkPosition.Value,
                    ref aiCharacter.characterNetworkManager.networkPositionVelocity,
                    aiCharacter.characterNetworkManager.networkPositionSmoothTime);
                aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
            }
        }

    }
}
