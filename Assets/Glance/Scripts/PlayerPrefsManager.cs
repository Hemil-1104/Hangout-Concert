using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public static class PlayerPrefsKeys
    {
        public static string XpPoints = "XpPoints";
        public static string TotalDanceMovesMastery = "TotalDanceMovesMastery";
    }

    public class PlayerPrefsManager : MonoBehaviour
    {
        private void Start()
        {
            XpProgressionManager.OnXpUpdated += XpProgressionManager_OnXpUpdated;

            DanceMasteryManager.OnDanceMoveMasteryUpdated += DanceMasteryManager_OnDanceMoveMasteryUpdated;
        }

        private void DanceMasteryManager_OnDanceMoveMasteryUpdated(float totalDanceMovesMastery)
        {
            PlayerPrefs.SetFloat(PlayerPrefsKeys.TotalDanceMovesMastery, totalDanceMovesMastery);
        }

        private void XpProgressionManager_OnXpUpdated(int xpAmount)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.XpPoints, xpAmount);
        }

        private void OnDestroy()
        {
            XpProgressionManager.OnXpUpdated -= XpProgressionManager_OnXpUpdated;

            DanceMasteryManager.OnDanceMoveMasteryUpdated -= DanceMasteryManager_OnDanceMoveMasteryUpdated;
        }

        public static int GetTotalXpPoints()
        {
            return PlayerPrefs.GetInt(PlayerPrefsKeys.XpPoints, 0);
        }

        public static float GetTotalDanceMovesMastery()
        {
            return PlayerPrefs.GetFloat(PlayerPrefsKeys.TotalDanceMovesMastery, 0f);
        }
    }
}
