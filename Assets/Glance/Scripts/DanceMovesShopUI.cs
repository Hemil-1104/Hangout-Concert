using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DanceMovesShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private DanceMoveButton danceMovePurchaseButtonPrefab;
        [SerializeField] private Transform danceMoveButtonsContainer;
        [SerializeField] private List<DanceMoveSO> purchasableDanceMovesList;

        [Header("UI Parameters")]
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        public static event Action<int> OnPurchaseDanceMove;

        private bool isShopSetup = false;

        private void Start()
        {
            openButton.onClick.AddListener(() =>
            {
                Show();
            });

            closeButton.onClick.AddListener(() =>
            {
                Hide(); 
            });
        }

        private void SetupDanceMoveButtons()
        {
            if (isShopSetup) return;

            isShopSetup = true;

            foreach(DanceMoveSO danceMoveSO in purchasableDanceMovesList)
            {
                int index = danceMoveSO.DanceMoveIndex;
                DanceMoveButton danceMoveButton = Instantiate(danceMovePurchaseButtonPrefab, danceMoveButtonsContainer);

                danceMoveButton.DanceMoveImage.sprite = danceMoveSO.DanceMoveSprite;
                danceMoveButton.DanceMoveCostText.text = danceMoveSO.DanceMoveCost.ToString();
                danceMoveButton.DanceMovePurchaseButton.onClick.AddListener(() =>
                {
                    HandlePurchaseDanceMoveWithIndex(index);
                    danceMoveButton.gameObject.SetActive(false);
                });
            }
        }

        private void HandlePurchaseDanceMoveWithIndex(int danceMoveIndex)
        {
            // handle coins requirement condition, when adding a coin flow

            OnPurchaseDanceMove?.Invoke(danceMoveIndex);
        }

        private void Show()
        {
            SetupDanceMoveButtons();

            mainContainer.SetActive(true);
        }

        private void Hide()
        {
            mainContainer.SetActive(false);
        }
    }
}
