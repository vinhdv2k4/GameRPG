using UnityEngine;
namespace TV{
    public class CharacterSoundFxManager : MonoBehaviour
    {
     private AudioSource audioSource;
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }
    }
}
