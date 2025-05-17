using System;
using MyGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DJBoothMinigameUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private DJBoothMinigameManager djBoothMinigameManager;

        [Header("UI Parameters")]
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button centerButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image crowdReactionBar;
        [SerializeField] private TextMeshProUGUI resultText;

        public static event Action OnGameClosed;

        private void Start()
        {
            DJBoothMinigameManager.OnPointsChanged += DJBoothMinigameManager_OnPointsChanged;
            DJBoothMinigameManager.OnATileDone += DJBoothMinigameManager_OnATileDone;

            DJBooth.OnStartedPlayingDJBoothGame += DJBooth_OnStartedPlayingDJBoothGame;

            closeButton.onClick.AddListener(() =>
            {
                OnGameClosed?.Invoke();

                ResetUI();
                Hide();
            });

            leftButton.onClick.AddListener(() =>
            {
                djBoothMinigameManager.HandleButtonClick(leftButton.GetComponent<RectTransform>(), 0);
            });

            centerButton.onClick.AddListener(() =>
            {
                djBoothMinigameManager.HandleButtonClick(centerButton.GetComponent<RectTransform>(), 1);
            });

            rightButton.onClick.AddListener(() =>
            {
                djBoothMinigameManager.HandleButtonClick(rightButton.GetComponent<RectTransform>(), 2);
            });
        }

        private void DJBooth_OnStartedPlayingDJBoothGame()
        {
            Show();
        }

        private void DJBoothMinigameManager_OnATileDone()
        {
            if (DJBoothMinigameManager.NumberOfTilesDone == DJBoothMinigameManager.NumberOfTilesToSpawn)
            {
                SetupResultText();
            }
            UpdateCrowdReactionMeter();
        }

        private void ResetUI()
        {
            crowdReactionBar.fillAmount = 0f;
            resultText.gameObject.SetActive(false);
        }

        private void DJBoothMinigameManager_OnPointsChanged(int points)
        {
            SetPointsText(points);
        }

        private void SetPointsText(int points)
        {
            pointsText.text = $"Points: {points}";
        }

        private void SetupResultText()
        {
            resultText.text = Utils.GetDJBoothMinigameResultText(DJBoothMinigameManager.Points, DJBoothMinigameManager.TotalPointsPossible);
            resultText.gameObject.SetActive(true);
        }

        private void UpdateCrowdReactionMeter()
        {
            crowdReactionBar.fillAmount = DJBoothMinigameManager.Points / (float)DJBoothMinigameManager.TotalPointsPossible;
        }

        private void Show()
        {
            mainContainer.SetActive(true);
        }

        private void Hide()
        {
            mainContainer.SetActive(false);
        }

        private void OnDestroy()
        {
            DJBoothMinigameManager.OnPointsChanged -= DJBoothMinigameManager_OnPointsChanged;
            DJBoothMinigameManager.OnATileDone -= DJBoothMinigameManager_OnATileDone;

            DJBooth.OnStartedPlayingDJBoothGame -= DJBooth_OnStartedPlayingDJBoothGame;
        }
    }
}