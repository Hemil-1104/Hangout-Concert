using System.Collections;
using TMPro;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class PopupsUI : MonoBehaviour
    {
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private float warningTimer;
        [SerializeField] private float warningDisappearTimer;

        [Header("Warning Texts")]
        [SerializeField] private TextMeshProUGUI tryingToScanUnlockedMoveText;

        private CanvasGroup canvasGroup;
        private GameObject currentActiveTextObject;
        private Coroutine hideWarningCoroutine;

        private void Awake()
        {
            canvasGroup = mainContainer.GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            Dancer.OnTryingToScanAlreadyUnlockedMove += Dancer_OnTryingToScanAlreadyUnlockedMove;
        }

        private void Dancer_OnTryingToScanAlreadyUnlockedMove()
        {
            Show();

            if (currentActiveTextObject != null)
            {
                currentActiveTextObject.SetActive(false);
            }

            tryingToScanUnlockedMoveText.gameObject.SetActive(true);
            currentActiveTextObject = tryingToScanUnlockedMoveText.gameObject;

            hideWarningCoroutine ??= StartCoroutine(HideWarning());
        }

        private void Show()
        {
            canvasGroup.alpha = 1f;
        }

        private void Hide()
        {
            canvasGroup.alpha = 0f;
        }

        private IEnumerator HideWarning()
        {
            yield return new WaitForSeconds(warningTimer);

            float time = 0f;

            while (time < warningDisappearTimer)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = 1f - time / warningDisappearTimer;

                yield return null;
            }

            Hide();
        }
    }
}
