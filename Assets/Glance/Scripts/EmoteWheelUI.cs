using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class EmoteWheelUI : MonoBehaviour
    {
        [SerializeField] private GameObject containerObject;
        [SerializeField] private PlayerDanceMovesManager playerDanceMovesManager;
        [SerializeField] private Transform danceMovesContainer;
        [SerializeField] private Button danceMoveButtonPrefab;

        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        public static event Action<int> OnDoDance;

        private List<DanceMoveSO> unlockedDanceMovesList;

        private void Start()
        {
            PlayerDanceMovesManager.OnNewDanceMoveUnlocked += PlayerDanceMovesManager_OnNewDanceMoveUnlocked;

            openButton.onClick.AddListener(() =>
            {
                Show();
            });

            unlockedDanceMovesList = playerDanceMovesManager.GetUnlockedDanceMovesList();
            SetupUnlockedDanceMoves();
        }

        private void SetupUnlockedDanceMoves()
        {
            foreach(DanceMoveSO danceMoveSO in unlockedDanceMovesList)
            {
                SetupDanceMoveButton(danceMoveSO);
            }
        }

        private void SetupDanceMoveButton(DanceMoveSO danceMoveSO)
        {
            Button danceMoveButton = Instantiate(danceMoveButtonPrefab, danceMovesContainer);
            Image danceIconImage = danceMoveButton.transform.GetChild(0).GetComponent<Image>();
            
            if(danceMoveSO.DanceMoveSprite != null)
            {
                danceIconImage.sprite = danceMoveSO.DanceMoveSprite;
            }

            danceMoveButton.onClick.AddListener(() =>
            {
                OnDoDance?.Invoke(danceMoveSO.DanceMoveIndex);
                Hide();
            });
        }

        private void PlayerDanceMovesManager_OnNewDanceMoveUnlocked(DanceMoveSO danceMoveSO)
        {
            SetupDanceMoveButton(danceMoveSO);
        }

        private void Show()
        {
            containerObject.SetActive(true);
        }

        private void Hide()
        {
            containerObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerDanceMovesManager.OnNewDanceMoveUnlocked -= PlayerDanceMovesManager_OnNewDanceMoveUnlocked;
        }
    }
}