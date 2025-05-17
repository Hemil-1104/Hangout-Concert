using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DanceMoveScanUI : MonoBehaviour
    {
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private GameObject scanningInfoContainer;

        [SerializeField] private Button closeButton;
        [SerializeField] private Image learnProgressBar;
        [SerializeField] private TextMeshProUGUI scanningDoneText;

        [SerializeField] private float moveLearnTime = 3f;

        public static event Action<int> OnLearnedDanceMove;
        public static event Action OnGetOutOfScanMode;

        private Coroutine moveLearningCoroutine;
        private Dancer dancerBeingScanned;

        private void Start()
        {
            Dancer.OnScanDancer += Dancer_OnScanDancer;

            closeButton.onClick.AddListener(() =>
            {
                OnGetOutOfScanMode?.Invoke();
                if(moveLearningCoroutine != null)
                {
                    StopCoroutine(moveLearningCoroutine);
                }
                Hide();
            });
        }

        private void Dancer_OnScanDancer(Transform dancerTransform)
        {
            Show();

            dancerBeingScanned = dancerTransform.GetComponent<Dancer>();
            moveLearningCoroutine = StartCoroutine(StartMoveLearningTimer());
        }

        private IEnumerator StartMoveLearningTimer()
        {
            float time = 0f;
            while(time < moveLearnTime)
            {
                time += Time.deltaTime;
                learnProgressBar.fillAmount = time / moveLearnTime;

                yield return null;
            }

            scanningDoneText.gameObject.SetActive(true);
            scanningInfoContainer.SetActive(false);

            OnLearnedDanceMove?.Invoke(dancerBeingScanned.GetCurrentDanceEmoteIndex());
        }

        private void Show()
        {
            mainContainer.SetActive(true);
            scanningDoneText.gameObject.SetActive(false);
            scanningInfoContainer.SetActive(true);
        }

        private void Hide()
        {
            mainContainer.SetActive(false);
        }

        private void OnDestroy()
        {
            Dancer.OnScanDancer -= Dancer_OnScanDancer;
        }
    }
}
