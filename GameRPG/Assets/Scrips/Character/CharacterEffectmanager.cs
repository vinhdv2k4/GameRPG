using UnityEngine;
namespace TV
{
    public class CharacterEffectmanager : MonoBehaviour
    {

        CharacterManager character;
        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public virtual void ProcessInstantEffect(CharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

       
    }
}
