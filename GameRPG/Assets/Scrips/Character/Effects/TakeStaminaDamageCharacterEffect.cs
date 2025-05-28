using UnityEngine;
namespace TV {
    [CreateAssetMenu(menuName = "Character Effects / Instant Effects/ Take stamina damage")]
    public class TakeStaminaDamageCharacterEffect : CharacterEffect
    {
        public float staminaDamage;
        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }
    }
}