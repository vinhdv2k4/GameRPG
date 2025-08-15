using UnityEngine;
using UnityEngine.AI;
namespace TV
{
    public class CharacterAIManager : CharacterManager
    {
        [HideInInspector] public AiCharacterNetworkManager aiCharacterNetworkManager;
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AiCharacterLocomotionManager aiCharacterLocomotionManager;


        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        [SerializeField] AiState currentState;

        [Header("States")]
        public IdleState idle;
        public PersueTargetState persuaTarget;
        public CombatStanceState combatStance;
        public AttackState attack;

      

        protected override void Awake()
        {
            base.Awake();

            aiCharacterNetworkManager = GetComponent<AiCharacterNetworkManager>();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterLocomotionManager = GetComponent<AiCharacterLocomotionManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            idle = Instantiate(idle);
            persuaTarget = Instantiate(persuaTarget);

            currentState = idle;
        }

        protected override void Update()
        {
            base.Update();
            aiCharacterCombatManager.HandleRecovery(this);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if(IsOwner)
            ProcessStateMachine();
        }
        private void ProcessStateMachine()
        {
            AiState nextState = currentState?.Tick(this);

            if(nextState != null)
            {
                currentState = nextState;
            }
            navMeshAgent.transform.localPosition =Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if(aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUnityManager.instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetDirection);
                aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position,aiCharacterCombatManager.currentTarget.transform.position);
            }
            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if (remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aiCharacterNetworkManager.isMoving.Value= true;
                }
                else
                {
                    aiCharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }
    }
}
