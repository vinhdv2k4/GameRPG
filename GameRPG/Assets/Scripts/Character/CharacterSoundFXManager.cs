using UnityEngine;
namespace TV{
    public class CharacterSoundFxManager : MonoBehaviour
    {
     private AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void PlayerSoundFX(AudioClip soundFX, float volume = 1f, bool randomizePitch =true, float pitchRandom =0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);

            audioSource.pitch = 1f;

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }
        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }

        public virtual void PlayAttackGrunt()
        {
            PlayerSoundFX(WorldSoundFXManager.instance.ChoseRandomSFXFromArray(attackGrunts));
        }

        public virtual void PlayDamageGrunt()
        {
            PlayerSoundFX(WorldSoundFXManager.instance.ChoseRandomSFXFromArray(damageGrunts));
        }
    }
}
