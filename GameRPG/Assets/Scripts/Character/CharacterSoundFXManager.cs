using UnityEngine;
namespace TV{
    public class CharacterSoundFxManager : MonoBehaviour
    {
     private AudioSource audioSource;
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
    }
}
