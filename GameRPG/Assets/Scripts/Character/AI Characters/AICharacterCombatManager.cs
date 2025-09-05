using UnityEngine;
namespace TV
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        protected CharacterAIManager aiCharacter;
        [Header("Action Recovery")]
        public float actionRecoveryTimer = 0f;

        [Header("TargetImformation")]
        public float distanceFromTarget;
        public float viewableAngle;
        public Vector3 targetDirection;

        [Header("Detection")]
        public float detectionRadius = 5;
        public float minimumFOV = -35f;
        public float maximumFOV = 35f;

        [Header("Attack Rotation Speed")]
        public float attackRotationSpeed = 40f;


        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<CharacterAIManager>();
            lockOnTransform = GetComponentInChildren<LockOnTargetTranform>().transform;
        }
        public void FindATargetViaLineOfSight(CharacterAIManager aiCharacter)
        {
            if (currentTarget != null)
                return;
            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUnityManager.instance.GetCharacterLayers());
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
                if (targetCharacter == null)
                    continue;
                if (targetCharacter == aiCharacter)
                    continue;
                if (targetCharacter.isDead.Value)
                    continue;

                if (WorldUnityManager.instance.CanIDamageThisCharacter(aiCharacter.characterGroup, targetCharacter.characterGroup))
                {
                    Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                    if (angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                    {
                        if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position,
                            targetCharacter.characterCombatManager.lockOnTransform.position,
                            WorldUnityManager.instance.GetEnvironmentLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position,
                                targetCharacter.characterCombatManager.lockOnTransform.position, Color.red);

                        }
                        else
                        {
                            targetDirection = targetCharacter.transform.position - transform.position;
                            viewableAngle = WorldUnityManager.instance.GetAngleOfTarget(transform, targetDirection);
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                            PivotTowardsTarget(aiCharacter);
                        }
                    }
                }
            }
        }

     

        public void PivotTowardsTarget(CharacterAIManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return;

            if (viewableAngle >= 20 && viewableAngle <= 60)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_Right_45", true);
            }
            else if (viewableAngle <= -20 && viewableAngle >= -60)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_Left_45", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_Left_90", true);
            }
            else if (viewableAngle >= 111 && viewableAngle <= 145)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_Right_135", true);
            }
            else if (viewableAngle <= -111 && viewableAngle >= -145)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_Left_135", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayerTargetActionAnimation("Turn_Left_180", true);
            }
        }
        public void RotateTowardsAgent(CharacterAIManager aiCharacter)
        {
            if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }
        public void RotateTowardsTargetWhilstAttacking(CharacterAIManager aiCharacter)
        {
            if (currentTarget == null)
                return;
            if (!aiCharacter.characterLocomotionManager.canRotate)
                return;
            if (!aiCharacter.isPerformingAction)
                return;
            Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if(targetDirection == Vector3.zero)
                targetDirection = aiCharacter.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed* Time.deltaTime);
        }
        public void HandleRecovery(CharacterAIManager aiCharacter)
        {
            if (actionRecoveryTimer > 0)
            {
                if (!aiCharacter.isPerformingAction)
                {
                    actionRecoveryTimer -= Time.deltaTime;
                }
            }
        }
    }
}
