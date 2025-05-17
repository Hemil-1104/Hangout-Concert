using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class XpBarUI : MonoBehaviour
    {
        [SerializeField] private Image xpProgressBar;
        [SerializeField] private XpProgressionManager xpProgressionManager;
        [SerializeField] private TextMeshProUGUI levelNumberText;

        public void UpdateLevelText()
        {
            levelNumberText.text = (xpProgressionManager.GetCurrentLevel() + 1).ToString();
        }

        public void UpdateXpAmount(int xpAmount)
        {
            xpProgressBar.fillAmount = (float)(xpAmount / (float)xpProgressionManager.GetCurentLevelXpRequirement());

            if (xpProgressBar.fillAmount >= 1f)
            {
                xpProgressBar.fillAmount -= 1f;
                UpdateLevelText();
            }
        }
    }
}
