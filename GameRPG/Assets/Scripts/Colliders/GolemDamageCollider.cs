using UnityEngine;
namespace TV
{
    public class GolemDamageCollider : DamageCollider
    {
        [SerializeField] AiBossCharacterManager bossCharacter;

        protected override void Awake()
        {
            base.Awake();
            if (bossCharacter == null)
            {
                damageCollider = GetComponent<Collider>();
                bossCharacter = GetComponentInParent<AiBossCharacterManager>();
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
            damageEffect.angleHitFrom = Vector3.SignedAngle(bossCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);



            if (damageTarget.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    bossCharacter.NetworkObjectId,
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
