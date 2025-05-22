using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameXpStats gameXpStats;

#if UNITY_EDITOR
        [SerializeField] private bool isTesting;
#endif

        #region Photo Mode Parameters

        public static float PhotoModeXpEarningCooldown {  get; private set; }

        public static int PhotoModeMinimumXp {  get; private set; }

        public static int PhotoModeMaximumXp { get; private set; }

        public static AnimationCurve PhotoModeXpIncreaseCurve { get; private set; }

        #endregion

        #region Clothes Styling Booth Parameters

        public static int ClothesStylingBoothXpReward {  get; private set; }

        public static float ClothesStylingBoothXpEarningCooldown { get; private set; }

        #endregion

        #region Dance Moves Parameters

        public static float DanceMovesXpEarningCooldown { get; private set; }

        #endregion

        #region DJ Booth Minigame Parameters

        public static int DJBoothMinigameMaximumXp { get; private set; }

        public static float DJBoothMinigameXpEarningCooldown { get; private set; }

        #endregion

        private void Awake()
        {
            PhotoModeXpEarningCooldown = gameXpStats.PhotoModeXpEarningCooldown;
            PhotoModeMinimumXp = gameXpStats.PhotoModeMinimumXp;
            PhotoModeMaximumXp = gameXpStats.t_PhotoModeMaximumXp;
            PhotoModeXpIncreaseCurve = gameXpStats.PhotoModeXpIncreaseCurve;

            ClothesStylingBoothXpReward = gameXpStats.ClothesStylingBoothXpReward;
            ClothesStylingBoothXpEarningCooldown = gameXpStats.ClothesStylingBoothXpEarningCooldown;

            DanceMovesXpEarningCooldown = gameXpStats.DanceMovesXpEarningCooldown;

            DJBoothMinigameMaximumXp = gameXpStats.DjBoothMinigameMaximumXp;
            DJBoothMinigameXpEarningCooldown = gameXpStats.DjBoothMinigameXpEarningCooldown;

#if UNITY_EDITOR

            PhotoModeXpEarningCooldown = isTesting ? gameXpStats.t_PhotoModeXpEarningCooldown : gameXpStats.PhotoModeXpEarningCooldown;
            PhotoModeMinimumXp = isTesting ? gameXpStats.t_PhotoModeMinimumXp : gameXpStats.PhotoModeMinimumXp;
            PhotoModeMaximumXp = isTesting ? gameXpStats.t_PhotoModeMaximumXp : gameXpStats.t_PhotoModeMaximumXp;
            PhotoModeXpIncreaseCurve = isTesting ? gameXpStats.t_PhotoModeXpIncreaseCurve : gameXpStats.PhotoModeXpIncreaseCurve;

            ClothesStylingBoothXpReward = isTesting ? gameXpStats.t_ClothesStylingBoothXpReward : gameXpStats.ClothesStylingBoothXpReward;
            ClothesStylingBoothXpEarningCooldown = isTesting ? gameXpStats.t_ClothesStylingBoothXpEarningCooldown : gameXpStats.ClothesStylingBoothXpEarningCooldown;

            DanceMovesXpEarningCooldown = isTesting ? gameXpStats.t_DanceMovesXpEarningCooldown : gameXpStats.DanceMovesXpEarningCooldown;

            DJBoothMinigameMaximumXp = isTesting ? gameXpStats.t_DjBoothMinigameMaximumXp : gameXpStats.DjBoothMinigameMaximumXp;
            DJBoothMinigameXpEarningCooldown = isTesting ? gameXpStats.t_DjBoothMinigameXpEarningCooldown : gameXpStats.DjBoothMinigameXpEarningCooldown;

#endif
        }
    }
}
