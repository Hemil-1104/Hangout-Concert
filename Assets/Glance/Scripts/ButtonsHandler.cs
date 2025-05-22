using System;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class ButtonsHandler : MonoBehaviour
    {
        [SerializeField] private Button stopDanceButton;
        [SerializeField] private Button capturePhotoButton;

        public static event Action OnStopDancing;

        private void Start()
        {
            PlayerAnimator.OnStartedDancing += PlayerAnimator_OnStartedDancing;

            stopDanceButton.onClick.AddListener(() =>
            {
                OnStopDancing?.Invoke();

                Hide(stopDanceButton.gameObject);
            });
        }

        private void PlayerAnimator_OnStartedDancing()
        {
            Show(stopDanceButton.gameObject);
        }

        private void Show(GameObject buttonObject)
        {
            buttonObject.SetActive(true);
        }

        private void Hide(GameObject buttonObject)
        {
            buttonObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerAnimator.OnStartedDancing -= PlayerAnimator_OnStartedDancing;
        }
    }
}