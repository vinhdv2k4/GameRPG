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
        public float angleHitFrom;
        public Vector3 contactPoint;
        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if(character.isDead.Value)
                return;

            CaculateDamage(character);
            PlayDirectionalBasedDamageAnimation(character);
            
            PlayDamageVFX(character);

            PlayeDamageSFX(character);
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


        private void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectManager.PlayBloodSpatterVFX(contactPoint);
        }
        private void PlayeDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChoseRandomSFXFromArray(WorldSoundFXManager.instance.PhysicdamageSFX);
            character.characterSoundFxManager.PlayerSoundFX(physicalDamageSFX);
        }

        private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            poiseIsBroken = true;
            if (angleHitFrom>= 145 && angleHitFrom <= 180)
            {
                damageAnimationName = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom<=-145 && angleHitFrom <=-180)
            {
                damageAnimationName = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if ( angleHitFrom>=-45 && angleHitFrom <=45)
            {
                damageAnimationName = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.back_Medium_Damage);

            }
            else if ((angleHitFrom>=-144 && angleHitFrom <-45))
            {
                damageAnimationName = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);


            }
            else if (angleHitFrom>=45 && angleHitFrom <=145)
            {
                damageAnimationName = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);

            }
            if (poiseIsBroken)
            {
                character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimationName;
                character.characterAnimatorManager.PlayerTargetActionAnimation(damageAnimationName,true);
            }
        }
    }

}