using UnityEngine;
namespace TV
{
    public class AiUndeadCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] UndeadHandDamageCollider rightHandDamageCollider;
        [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1f;
        [SerializeField] float attack02DamageModifier = 1.5f;

        public void SetAttack01Damage()
        {
            rightHandDamageCollider.physicDamage = baseDamage * attack01DamageModifier;
            leftHandDamageCollider.physicDamage = baseDamage * attack01DamageModifier;
        }
        public void SetAttack02Damage()
        {
            rightHandDamageCollider.physicDamage = baseDamage * attack02DamageModifier;
            leftHandDamageCollider.physicDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenRightHandDamageCollider()
        {
            aiCharacter.characterSoundFxManager.PlayAttackGrunt();
            rightHandDamageCollider.EnableDamageCollider();
        }
        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        public void OpenLeftHandDamageCollider()
        {
            aiCharacter.characterSoundFxManager.PlayAttackGrunt();
            leftHandDamageCollider.EnableDamageCollider();
        }
        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }
}
