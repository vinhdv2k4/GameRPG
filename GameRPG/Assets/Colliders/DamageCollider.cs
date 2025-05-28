using Unity.VisualScripting;
using System.Collections.Generic;

using UnityEngine;
namespace TV
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")]
        public float physicDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        public float poisonDamage = 0;

        [Header("Contact Point")]
        private Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();
        public void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponent<CharacterManager>();
            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;
            charactersDamaged.Add(damageTarget);

            TakeDamageEffect takeDamageEffect =Instantiate(WorldCharacterEffectManager.instance.takeDamageEffect);
            takeDamageEffect.physicDamage = physicDamage;
            takeDamageEffect.magicDamage = magicDamage;
            takeDamageEffect.fireDamage = fireDamage;
            takeDamageEffect.lightningDamage = lightningDamage;
            takeDamageEffect.holyDamage = holyDamage;
            takeDamageEffect.poisonDamage = poisonDamage;
            takeDamageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectManager.ProcessInstantEffect(takeDamageEffect);
        }
       



    }
}
