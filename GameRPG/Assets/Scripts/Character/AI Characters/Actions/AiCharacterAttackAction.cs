using UnityEngine;
namespace TV
{
    [CreateAssetMenu(menuName = "A.I/Actions/Attack")]
    public class AiCharacterAttackAction : ScriptableObject
    {
        [Header("Attack")]
        [SerializeField] private string attackAnimation;

        [Header("Combo action")]
        public AiCharacterAttackAction comboAction;

        [Header("Action Settings")]
        public int attackWeight = 50;
        [SerializeField] AttackStyle attackStyle;
        public float actionRecoveryTime = 1.5f;
        public float minimumAttackAngle = -35f;
        public float maximumAttackAngle = 35f;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2f;
        public void AttemptToPerformAction(CharacterAIManager aiCharacter)
        {
            aiCharacter.characterAnimatorManager.PlayerTargetAttackActionAnimation(attackStyle, attackAnimation, true);
        }
    }
}
