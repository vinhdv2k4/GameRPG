using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace TV {
    public class PlayerUiPopUpManager : MonoBehaviour
    {
        [Header("YOU Die pop up")]
        [SerializeField] GameObject youDiePopUpObject;
        [SerializeField] TextMeshProUGUI youDiePopUpBackgrounddText;
        [SerializeField] TextMeshProUGUI youDiePopUpText;
        [SerializeField] CanvasGroup youDiePopUpCanvasGroup;

        public void SendYouDiePopUp()
        {
            youDiePopUpObject.SetActive(true);
            youDiePopUpBackgrounddText.characterSpacing = 0;
            StartCoroutine(StretchPopupTextOverTime(youDiePopUpBackgrounddText,8,20));
            StartCoroutine(FadeInPopUpOverTime(youDiePopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiePopUpCanvasGroup, 2, 5));
        }

        private IEnumerator StretchPopupTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if(duration > 0)
            {
                text.characterSpacing = 0;
                float timer = 0;
                yield return null;
                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration *(Time.deltaTime /20));
                    yield return null;
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration>0)
            {
                canvas.alpha = 0;
                float timer = 0;
                yield return null;

                while (timer< duration)
                {
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration *Time.deltaTime );
                    yield return null;

                }
            }
            canvas.alpha = 1;
            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0)
            {
                while (delay > 0){
                    delay -= Time.deltaTime;
                    yield return null;
                }
                canvas.alpha = 1;
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration*Time.deltaTime);
                    yield return null;

                }
            }
            canvas.alpha = 0;
            yield return null;
        }
    }
}