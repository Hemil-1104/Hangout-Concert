using System.Collections.Generic;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DanceMovesXpManager : XPEarningMethod
    {
        [SerializeField] private DanceMasteryManager danceMasteryManager;
        [SerializeField] private List<DanceMoveSO> availableDanceMovesList;
        [SerializeField] private float startingMultiplier = 0.5f;

        private int currentDanceMoveIndex = 0;

        protected override void Start()
        {
            EmoteWheelUI.OnDoDance += EmoteWheelUI_OnDoDance;

            ButtonsHandler.OnStopDancing += ButtonsHandler_OnStopDancing;

            xpEarningCooldown = GameManager.DanceMovesXpEarningCooldown;

            base.Start();
        }

        private void ButtonsHandler_OnStopDancing()
        {
            if (canEarnXp)
            {
                int totalXpReward = Mathf.RoundToInt(availableDanceMovesList[currentDanceMoveIndex].DanceMoveXpReward * (startingMultiplier + (danceMasteryManager.GetTotalDanceMovesMasteryNormalized() / 2f)));

                EarnXp(totalXpReward);
                StartCoroutine(StartCooldown());
            }
        }

        private void EmoteWheelUI_OnDoDance(int danceMoveIndex)
        {
            currentDanceMoveIndex = danceMoveIndex;
        }

        private void OnDestroy()
        {
            EmoteWheelUI.OnDoDance -= EmoteWheelUI_OnDoDance;

            ButtonsHandler.OnStopDancing -= ButtonsHandler_OnStopDancing;
        }
    }
}