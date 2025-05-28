using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
namespace TV
{
    public class WorldCharacterEffectManager : MonoBehaviour
    {
        public static WorldCharacterEffectManager instance;

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;
        [SerializeField] List<CharacterEffect> instantEffects;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            GenerateEffectIDs();
        }
        
        private void GenerateEffectIDs()
        {
            for (int i =0; i < instantEffects.Count; i++)
            {
                instantEffects[i].EffectID = i;
            }
        }

    }
}