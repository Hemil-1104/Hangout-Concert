using System.Text;
using TMPro;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class LevelTextUI : MonoBehaviour
    {
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private XpProgressionManager xpProgressionManager;

        [SerializeField] private TextMeshProUGUI levelWarningText;

        private string originalText;

        private void Awake()
        {
            originalText = levelWarningText.text;
        }

        private void Start()
        {
            LevelPromptTrigger.OnPlayerEnteredLevelTrigger += LevelPromptTrigger_OnPlayerEnteredLevelTrigger;
            LevelPromptTrigger.OnPlayerExitedLevelTrigger += LevelPromptTrigger_OnPlayerExitedLevelTrigger;
        }

        private void LevelPromptTrigger_OnPlayerExitedLevelTrigger()
        {
            Hide();   
        }

        private void LevelPromptTrigger_OnPlayerEnteredLevelTrigger()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(originalText, xpProgressionManager.GetRemainingXpAmountToReachNextLevel());

            levelWarningText.text = stringBuilder.ToString();

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
            LevelPromptTrigger.OnPlayerEnteredLevelTrigger -= LevelPromptTrigger_OnPlayerEnteredLevelTrigger;
            LevelPromptTrigger.OnPlayerExitedLevelTrigger -= LevelPromptTrigger_OnPlayerExitedLevelTrigger;
        }
    }
}