using UnityEngine;
using UnityEngine.UI;
namespace TV
{
    public class UI_Sta_Bars : MonoBehaviour
    {
        private Slider slider;
        private RectTransform rectTransform;

        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWithStats =true;
        [SerializeField] protected float widthScaleMultiplier = 1f;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }
        public virtual void SetSta(int newValue)
        {
            slider.value = newValue;
        }
        public virtual void SetMaxSta(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
            if (scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                PlayerUIManger.instance.playerUIHudManager.RefeshHud();
            }
        }
    }
}
