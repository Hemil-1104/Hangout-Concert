using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    [CreateAssetMenu()]
    public class GameXpStats : ScriptableObject
    {
        [Header("Actual Game Values")]

        public float PhotoModeXpEarningCooldown;
        public int PhotoModeMaximumXp;
        public int PhotoModeMinimumXp;
        public AnimationCurve PhotoModeXpIncreaseCurve;

        public float DjBoothMinigameXpEarningCooldown;
        public int DjBoothMinigameMaximumXp;

        public int ClothesStylingBoothXpReward;
        public float ClothesStylingBoothXpEarningCooldown;

        public float DanceMovesXpEarningCooldown;

        [Header("Testing Values")]

        public float t_PhotoModeXpEarningCooldown;
        public int t_PhotoModeMaximumXp;
        public int t_PhotoModeMinimumXp;
        public AnimationCurve t_PhotoModeXpIncreaseCurve;

        public float t_DjBoothMinigameXpEarningCooldown;
        public int t_DjBoothMinigameMaximumXp;

        public int t_ClothesStylingBoothXpReward;
        public float t_ClothesStylingBoothXpEarningCooldown;

        public float t_DanceMovesXpEarningCooldown;
    }
}