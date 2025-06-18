using NUnit.Framework;
using System.Collections.Generic;
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

        [Header("Damage Animations")]
        public string lastDamageAnimationPlayed;
        [SerializeField] string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
        [SerializeField] string hit_Forward_Medium_02 = "hit_Forward_Medium_02";

        [SerializeField] string hit_Back_Medium_01 = "hit_Back_Medium_01";
        [SerializeField] string hit_Back_Medium_02 = "hit_Back_Medium_02";

        [SerializeField] string hit_Left_Medium_01 = "hit_Left_Medium_01";
        [SerializeField] string hit_Left_Medium_02 = "hit_Left_Medium_02";

        [SerializeField] string hit_Right_Medium_01 = "hit_Right_Medium_01";
        [SerializeField] string hit_Right_Medium_02 = "hit_Right_Medium_02";


        public List<string> forward_Medium_Damage = new List<string>();
        public List<string> back_Medium_Damage = new List<string>();
        public List<string> left_Medium_Damage = new List<string>();
        public List<string> right_Medium_Damage = new List<string>();


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        protected virtual void Start()
        {
            // Initialize damage animations
            forward_Medium_Damage.Add(hit_Forward_Medium_01);
            forward_Medium_Damage.Add(hit_Forward_Medium_02);
            back_Medium_Damage.Add(hit_Back_Medium_01);
            back_Medium_Damage.Add(hit_Back_Medium_02);
            left_Medium_Damage.Add(hit_Left_Medium_01);
            left_Medium_Damage.Add(hit_Left_Medium_02);
            right_Medium_Damage.Add(hit_Right_Medium_01);
            right_Medium_Damage.Add(hit_Right_Medium_02);
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();
            foreach (var item in animationList)
            {
                finalList.Add(item); 
        }

        finalList.Remove(lastDamageAnimationPlayed);
        for(int i = finalList.Count -1;i>-1;i--)
        {
            if(finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }
        int randomValue = Random.Range(0, finalList.Count);
            return finalList[randomValue];
        }
    
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            // du lieu thay doi ma ko thay doi du lieu goc

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
            Debug.Log($"PlayerTargetActionAnimation: {targetAnimation}");
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

        public virtual void PlayerTargetAttackActionAnimation( AttackStyle attackStyle,
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canMove = false,
            bool canRotate = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);

            // stop character attemting new action, if main get damage and beging a damage action, this frag will true
            character.characterCombatManager.currentAttackSyle = attackStyle;
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;
            character.characterNetworkManager.NotifiTheServerOfAttackActionAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId,
                targetAnimation,
                applyRootMotion);
        }
    }
}

