using UnityEngine;
using Unity.Netcode;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
namespace TV
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectmanager characterEffectManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;

        [Header("Flags")]
        public bool isPerformingAction;
        public bool applyRootMotion = false;
        public bool canMove = true;
        public bool canRotate = true;
        public bool isJumping = false;
        public bool isGrounded = true;



        protected virtual void Awake()
        {

            DontDestroyOnLoad(this);
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectManager = GetComponent<CharacterEffectmanager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }
        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", isGrounded);
            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);

                transform.rotation = Quaternion.Slerp(transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;
                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true);
                }
            }
            yield return new WaitForSeconds(5);
        }

        public virtual void ReviveCharacter()
        {
            if (IsOwner)
            {

            }
        }
    }
}
