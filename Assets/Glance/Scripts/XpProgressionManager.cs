using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class XpProgressionManager : MonoBehaviour
    {
        [SerializeField] private List<int> xpRequirementsList;
        [SerializeField] private List<GameObject> areaBlockingWallsList;
        [SerializeField] private XpBarUI xpBarUI;

        public static event Action<int> OnXpUpdated;

        private int receivedXpAmount = 0;
        private int remainingXpToUpdate = 0;
        private int currentLevel = 0;
        private int currentXp = 0;
        private int currentLevelXpRequirement;
        private int totalLevels;

        private void Awake()
        {
            currentLevelXpRequirement = xpRequirementsList[currentLevel];
            totalLevels = xpRequirementsList.Count;
        }

        private void Start()
        {
            XPEarningMethod.OnEarnedXp += XPEarningMethod_OnEarnedXp;

            XpBarUI.OnXpBarReachedTargetAmount += XpBarUI_OnXpBarReachedTargetAmount;

            InitializeXpPoints();
        }

        private void XpBarUI_OnXpBarReachedTargetAmount()
        {
            if(remainingXpToUpdate > 0)
            {
                int xpAmount = remainingXpToUpdate;
                remainingXpToUpdate = 0;
                AddXP(xpAmount);
            }
        }

        private void XPEarningMethod_OnEarnedXp(int xpReward)
        {
            AddXP(xpReward);
        }

        private void InitializeXpPoints()
        {
            currentXp = PlayerPrefsManager.GetTotalXpPoints();
            CheckIfAnyRequirementFulfilled();
            xpBarUI.UpdateXpAmount(currentXp, isInitializing: true);
        }

        private void AddXP(int xpAmount)
        {
            if (currentLevel == totalLevels) return;

            CheckIfXpRequirementFulfilled();

            if ((currentXp + xpAmount) > currentLevelXpRequirement)
            {
                remainingXpToUpdate = xpAmount - (currentLevelXpRequirement - currentXp);
                receivedXpAmount = (currentLevelXpRequirement - currentXp);
                currentXp = currentLevelXpRequirement;
            }
            else
            {
                receivedXpAmount = xpAmount;
                currentXp += xpAmount;
            }

            OnXpUpdated?.Invoke(currentXp);
            xpBarUI.UpdateXpAmount(currentXp, isInitializing: false);
        }

        private void CheckIfAnyRequirementFulfilled()
        {
            for (int i = 0; i < xpRequirementsList.Count; i++)
            {
                if (currentXp >= xpRequirementsList[i])
                {
                    // Unlock region based on the xp requirement being fulfilled
                    areaBlockingWallsList[i].SetActive(false);
                    currentLevel += 1;
                    currentLevelXpRequirement = xpRequirementsList[currentLevel];

                    xpBarUI.UpdateLevelText();
                }
            }
        }

        private void CheckIfXpRequirementFulfilled()
        {
            if(currentXp >= xpRequirementsList[currentLevel])
            {
                // Unlock region based on the xp requirement being fulfilled
                areaBlockingWallsList[currentLevel].SetActive(false);
                currentLevel += 1;

                if (currentLevel != totalLevels)
                {
                    currentLevelXpRequirement = xpRequirementsList[currentLevel];
                }

                xpBarUI.UpdateLevelText();
            }
        }

        private void OnDestroy()
        {
            XPEarningMethod.OnEarnedXp -= XPEarningMethod_OnEarnedXp;

            XpBarUI.OnXpBarReachedTargetAmount -= XpBarUI_OnXpBarReachedTargetAmount;
        }

        public int GetCurrentLevelXpRequirement()
        {
            return currentLevelXpRequirement;
        }

        public int GetReceivedXpAmount()
        {
            return receivedXpAmount;
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        public int GetRemainingXpAmountToReachNextLevel()
        {
            return currentLevelXpRequirement - currentXp;
        }
    }
}