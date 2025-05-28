using UnityEngine;
using UnityEngine.TextCore.Text;
namespace TV
{
    [CreateAssetMenu(menuName = "Character Effects / Instant Effects/ Take  damage")]

    public class TakeDamageEffect : CharacterEffect
    {
        [Header("Character Damage")]
        public CharacterManager characterCasingDamage;

        [Header("Damage")]
        public float physicDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        public float poisonDamage = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("Animation")]
        public bool playerDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimationName;
        [Header ("Final Damage")]
        public int finalPhysicDamage = 0;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;

        [Header("Direction Damage Taken From")]
        public float angleHitfROM;
        public Vector3 contactPoint;
        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if(character.isDead.Value)
                return;

            CaculateDamage(character);
        }
        private void CaculateDamage(CharacterManager character)
        {
            if(!character.IsOwner)
                return;
            if (characterCasingDamage != null)
            {

            }
            finalPhysicDamage = Mathf.RoundToInt(physicDamage+magicDamage+fireDamage+lightningDamage+holyDamage+poisonDamage);
            if (finalPhysicDamage <= 0)
            {
                finalPhysicDamage = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalPhysicDamage;
        }
    }

}