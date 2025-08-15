using JetBrains.Annotations;
using UnityEngine;
namespace TV {
    public class UndeadHandDamageCollider : DamageCollider
    {
        [SerializeField] CharacterAIManager undeadCharacter;

        protected override void Awake()
        {
            base.Awake();
            if (undeadCharacter == null)
            {
                damageCollider = GetComponent<Collider>();
                undeadCharacter = GetComponentInParent<CharacterAIManager>();
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
            damageEffect.angleHitFrom = Vector3.SignedAngle(undeadCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);

           

            if (damageTarget.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    undeadCharacter.NetworkObjectId,
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

    }
}
