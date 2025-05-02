using UnityEngine;
using UnityEngine.UI;
namespace TV
{
    public class UI_Sta_Bars : MonoBehaviour
    {
        private Slider slider;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }
        public virtual void SetSta(int newValue)
        {
            slider.value = newValue;
        }
        public virtual void SetMaxSta(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
}
