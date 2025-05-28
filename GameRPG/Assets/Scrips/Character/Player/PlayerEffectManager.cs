using UnityEngine;
namespace TV
{
    public class PlayerEffectManager : CharacterEffectmanager
    {
        [Header("Debug Delete Later")]
        [SerializeField] TakeStaminaDamageCharacterEffect effectToTest;
        [SerializeField] bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                effectToTest.staminaDamage = 30;
                ProcessInstantEffect(effectToTest);
            }
        }   
    }
}
