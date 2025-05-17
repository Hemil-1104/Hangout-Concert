using System.Collections.Generic;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class PhotoModeManager : XPEarningMethod
    {
        [SerializeField] private CharacterVisualsManager characterVisualsManager;

        private int minimumXp;
        private int maximumXp;
        private AnimationCurve xpIncreaseCurve;

        private bool isCharacterVisualsListSetup = false;
        private List<CharacterVisual> characterVisualsList;

        protected override void Start()
        {
            minimumXp = GameManager.PhotoModeMinimumXp;
            maximumXp = GameManager.PhotoModeMaximumXp;
            xpEarningCooldown = GameManager.PhotoModeXpEarningCooldown;
            xpIncreaseCurve = GameManager.PhotoModeXpIncreaseCurve;

            PhotoModeCanvasUI.OnPhotoCaptured += PhotoModeCanvasUI_OnPhotoCaptured;

            base.Start();
        }

        private void PhotoModeCanvasUI_OnPhotoCaptured()
        {
            if (canEarnXp)
            {
                int xpReward = minimumXp;
                int numberOfCharactersVisibleInShot = 0;

                if (!isCharacterVisualsListSetup)
                {
                    isCharacterVisualsListSetup = true;
                    characterVisualsList = characterVisualsManager.GetCharacterVisualsList();
                }

                foreach (CharacterVisual characterVisual in characterVisualsList)
                {
                    Renderer characterRenderer = characterVisual.GetCharacterRenderer();

                    if (characterRenderer.isVisible)
                    {
                        numberOfCharactersVisibleInShot += 1;
                        xpReward += Mathf.RoundToInt(xpIncreaseCurve.Evaluate(numberOfCharactersVisibleInShot));

                        xpReward = Mathf.Clamp(xpReward, minimumXp, maximumXp);
                    }
                }

                if (numberOfCharactersVisibleInShot == 0)
                {
                    xpReward = 0;
                }
                else
                {
                    StartCoroutine(StartCooldown());
                }

                EarnXp(xpReward);
            }
        }

        private void OnDestroy()
        {
            PhotoModeCanvasUI.OnPhotoCaptured -= PhotoModeCanvasUI_OnPhotoCaptured;
        }
    }
}
