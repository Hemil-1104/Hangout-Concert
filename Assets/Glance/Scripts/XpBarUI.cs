using System;
using System.Collections;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class XpBarUI : MonoBehaviour
    {
        [SerializeField] private Image xpProgressBar;
        [SerializeField] private XpProgressionManager xpProgressionManager;
        [SerializeField] private TextMeshProUGUI levelNumberText;

        [Header("Xp Bar Lerp Parameters")]
        [SerializeField] private float xpBarFillingSpeed = 1f;
        [SerializeField] private float minimumDistance = 0.1f;

        [Header("Xp Increase UI Paramaters")]
        [SerializeField] private TextMeshProUGUI xpIncreaseText;
        [SerializeField] private float delayBeforeFadingOut = 2f;
        [SerializeField] private float fadeTimer = 1f;

        public static event Action OnXpBarReachedTargetAmount;

        private Coroutine xpReceivedTextFadeCoroutine;
        private CanvasGroup xpIncreaseTextCanvasGroup;
        private string originalXpIncreaseText;

        private void Awake()
        {
            originalXpIncreaseText = xpIncreaseText.text;
            xpIncreaseTextCanvasGroup = xpIncreaseText.GetComponent<CanvasGroup>();

            xpIncreaseTextCanvasGroup.alpha = 0f;
        }

        private IEnumerator LerpFillAmount(float fillAmount, Action OnFinished)
        {
            while(Mathf.Abs(xpProgressBar.fillAmount - fillAmount) > minimumDistance)
            {
                xpProgressBar.fillAmount = Mathf.Lerp(xpProgressBar.fillAmount, fillAmount, xpBarFillingSpeed * Time.deltaTime);

                yield return null;
            }

            OnFinished?.Invoke();
            OnXpBarReachedTargetAmount?.Invoke();
        }

        private IEnumerator FadeOutReceivedXpText()
        {
            yield return new WaitForSeconds(delayBeforeFadingOut);

            xpIncreaseTextCanvasGroup.DOFade(0f, fadeTimer);
        }

        private void OnReachedTargetAmount()
        {
            if (Mathf.Abs(xpProgressBar.fillAmount - 1f) <= minimumDistance)
            {
                xpProgressBar.fillAmount = 0f;
                UpdateLevelText();
            }
        }

        private void UpdateReceviedXpAmountText()
        {
            if (xpReceivedTextFadeCoroutine != null)
            {
                StopCoroutine(xpReceivedTextFadeCoroutine);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(originalXpIncreaseText, xpProgressionManager.GetReceivedXpAmount());
            xpIncreaseText.text = stringBuilder.ToString();

            xpIncreaseTextCanvasGroup.alpha = 1f;
            xpReceivedTextFadeCoroutine = StartCoroutine(FadeOutReceivedXpText());
        }

        public void UpdateLevelText()
        {
            levelNumberText.text = (xpProgressionManager.GetCurrentLevel() + 1).ToString();
        }

        public void UpdateXpAmount(int xpAmount, bool isInitializing)
        {
            int currentLevelXpRequirement = xpProgressionManager.GetCurrentLevelXpRequirement();

            if (isInitializing)
            {
                xpProgressBar.fillAmount = (float)(xpAmount / (float)currentLevelXpRequirement);
                return;
            }
            
            float targetFillAmount = (float)(xpAmount / (float)currentLevelXpRequirement);

            UpdateReceviedXpAmountText();
            StartCoroutine(LerpFillAmount(targetFillAmount, OnReachedTargetAmount));
        }
    }
}