using System;
using System.Collections;
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

        protected override void Start()
        {
            EmoteWheelUI.OnDoDance += EmoteWheelUI_OnDoDance;

            xpEarningCooldown = GameManager.DanceMovesXpEarningCooldown;

            base.Start();
        }

        private void EmoteWheelUI_OnDoDance(int danceMoveIndex)
        {
            if(canEarnXp)
            {
                int totalXpReward = Mathf.RoundToInt(availableDanceMovesList[danceMoveIndex].DanceMoveXpReward * (startingMultiplier + (danceMasteryManager.GetTotalDanceMovesMasteryNormalized() / 2f)));

                //Debug.Log($"Total Xp Reward: {totalXpReward}");
                EarnXp(totalXpReward);
                StartCoroutine(StartCooldown());
            }
        }

        private void OnDestroy()
        {
            EmoteWheelUI.OnDoDance -= EmoteWheelUI_OnDoDance;
        }
    }
}