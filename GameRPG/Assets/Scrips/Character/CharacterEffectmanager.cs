using UnityEngine;
namespace TV
{
    public class CharacterEffectmanager : MonoBehaviour
    {

        CharacterManager character;

        [Header("VFX")]
        [SerializeField] GameObject bloodSpatterPrefab;
        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public virtual void ProcessInstantEffect(CharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

       public void PlayBloodSpatterVFX(Vector3 contactPoint)
        {
            if(bloodSpatterPrefab != null)
            {
                GameObject bloodSpatter = Instantiate(bloodSpatterPrefab, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSpatter = Instantiate(WorldCharacterEffectManager.instance.bloodSpatterPrefab, contactPoint, Quaternion.identity);
            }
        }
    }
}
