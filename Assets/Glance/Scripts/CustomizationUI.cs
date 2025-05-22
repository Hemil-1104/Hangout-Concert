using System;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class CustomizationUI : MonoBehaviour
    {
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private CustomizationManager customizationManager;

        [Header("UI")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button autoGenerateButton;

        public static event Action OnCustomizationScreenClosed;
        private void Start()
        {
            OutfitStylingBooth.OnEnterOutfitStylingBooth += OutfitStylingBooth_OnEnterOutfitStylingBooth;

            closeButton.onClick.AddListener(() =>
            {
                OnCustomizationScreenClosed?.Invoke();

                Hide();
            });

            autoGenerateButton.onClick.AddListener(() =>
            { 
                customizationManager.AutoGenerateOutfit();
                Hide();
            });
        }

        private void OutfitStylingBooth_OnEnterOutfitStylingBooth()
        {
            Show();
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
            OutfitStylingBooth.OnEnterOutfitStylingBooth -= OutfitStylingBooth_OnEnterOutfitStylingBooth;
        }
    }
}