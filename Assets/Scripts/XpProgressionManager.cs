using System;
using System.Collections.Generic;
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

            InitializeXpPoints();
        }

        private void XPEarningMethod_OnEarnedXp(int xpReward)
        {
            AddXP(xpReward);
        }

        private void InitializeXpPoints()
        {
            currentXp = PlayerPrefsManager.GetTotalXpPoints();
            CheckIfAnyRequirementFulfilled();
            xpBarUI.UpdateXpAmount(currentXp);
        }

        private void AddXP(int xpAmount)
        {
            if (currentLevel == totalLevels) return;

            currentXp += xpAmount;

            OnXpUpdated?.Invoke(currentXp);
            
            CheckIfXpRequirementFulfilled();
            xpBarUI.UpdateXpAmount(currentXp);
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
        }

        public int GetCurentLevelXpRequirement()
        {
            return currentLevelXpRequirement;
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }
    }
}