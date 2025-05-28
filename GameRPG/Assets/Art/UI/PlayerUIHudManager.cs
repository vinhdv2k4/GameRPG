using UnityEngine;
namespace TV
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] UI_Sta_Bars healthBar;
        [SerializeField] UI_Sta_Bars staminaBar;

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetSta(newValue);
        }
        public void SetMaxHealthValue(int maxhealth)
        {
            healthBar.SetMaxSta(maxhealth);
        }
        public void SetNewStaminaValue(float  oldValue ,float  newValue)
        {
            staminaBar.SetSta(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxSta(maxStamina);
        }

        public void RefeshHud()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
    }
}
