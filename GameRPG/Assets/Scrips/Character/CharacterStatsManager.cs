using System.Globalization;
using UnityEngine;
namespace TV
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character; 
        [Header("Stamina RegenarateStamina")]
        [SerializeField] float staminaRegenarationAmount = 1f;
        private float staminaRegenarationTimer = 0f;
        private float staminaTickTimer = 0f;
        [SerializeField] float staminaRegenarationDelay = 2f;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {
            
        }
        public int CaculateHealthBasedOnVitalityLevel(int vitality )
        {
            float health = 0;

            health = vitality * 10;
            return Mathf.RoundToInt(health);
        }
        public int CaculateStaminaBasedOnEnduranceLevel(int ebdurance)
        {
            float stamina = 0;

            stamina = ebdurance * 10;   
            return Mathf.RoundToInt(stamina);
        }
        public virtual void RegenarateStamina()
        {
            if (!character.IsOwner)
                return;

            if (character.characterNetworkManager.isSprinting.Value)
                return;

            if (character.isPerformingAction)
                return;

            staminaRegenarationTimer += Time.deltaTime;
            if (staminaRegenarationTimer >= staminaRegenarationDelay)
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;
                }
            if (staminaTickTimer >= 0.1f)
            {
                staminaTickTimer = 0f;
                character.characterNetworkManager.currentStamina.Value += staminaRegenarationAmount;
            }
        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            //use rest if the action used
            if(currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenarationTimer = 0f;
            }
            

        }
    }
}
