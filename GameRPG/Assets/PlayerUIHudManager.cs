using UnityEngine;
namespace TV
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] UI_Sta_Bars staminaBar;
        public void SetNewStaminaValue(float  oldValue ,float  newValue)
        {
            staminaBar.SetSta(Mathf.RoundToInt(newValue));
        }
        public void SetMaxStaminaValue(int maxValue)
        {
            staminaBar.SetMaxSta(maxValue);
        }
    }
}
