using UnityEngine;
namespace TV
{
    public class MeleeDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCasingDamage;

        [Header("Weapon Attack Modifiers ")]
        public float lightAttackModifier ;

        protected override void Awake()
        {
            base.Awake();

            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }
            damageCollider.enabled = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            
            if (damageTarget != null)
            {
                if (damageTarget == characterCasingDamage)
                    return;
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectManager.instance.takeDamageEffect);
            damageEffect.physicDamage = physicDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poisonDamage = poisonDamage;

            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCasingDamage.transform.forward, damageTarget.transform.forward,Vector3.up);

            switch (characterCasingDamage.characterCombatManager.currentAttackSyle)
            {
                case AttackStyle.lightAttack:
                    ApplyAttackDamnageModifiers(lightAttackModifier, damageEffect);
                    break;
                default:
                    break;
            }

            if (characterCasingDamage.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    characterCasingDamage.NetworkObjectId,
                    damageEffect.physicDamage,
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.lightningDamage,
                    damageEffect.holyDamage,
                    damageEffect.poisonDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    damageEffect.contactPoint.y,
                    damageEffect.contactPoint.z  

                );
            }

        }

        private void ApplyAttackDamnageModifiers(float modifier, TakeDamageEffect damage)
        {
           damage.physicDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.lightningDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.poisonDamage *= modifier;
        }
    }
}
