using UnityEngine;
namespace TV
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        private void OnAnimatorMove()
        {
            if (player.applyRootMotion)
            {
                 Vector3 volocity = player.animator.deltaPosition;
                player.characterController.Move(volocity);
                player.transform.rotation *= player.animator.deltaRotation;   
            }
        }
    }
}
