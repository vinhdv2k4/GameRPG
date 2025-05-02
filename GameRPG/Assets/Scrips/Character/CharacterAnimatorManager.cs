using Unity.IO.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
namespace TV
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;
        int horizontal;
        int vertical;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;
            if (isSprinting)
            {
                verticalAmount = 2f;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }



        public virtual void PlayerTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canMove = false, 
            bool canRotate =false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);

            // stop character attemting new action, if main get damage and beging a damage action, this frag will true
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;
            character.characterNetworkManager.NotifiTheServerActionAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId,
                targetAnimation,
                applyRootMotion);
        }
    }
}
