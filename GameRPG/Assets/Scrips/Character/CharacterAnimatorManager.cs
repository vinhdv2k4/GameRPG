using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
namespace TV
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;
        float horizontal;
        float vertical;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public void UpadateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            
            character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
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
        }
    }
}
