using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DanceMasteryManager : MonoBehaviour
    {
        [SerializeField] private PlayerDanceMovesManager playerDanceMovesManager;
        [SerializeField] private float totalMasteryPoints;
        [SerializeField] private List<AvatarMaskBodyPart> defaultActiveBodyPartsList;

#if UNITY_EDITOR
        [Header("Testing Parameters")]
        [Min(1f)] [SerializeField] private float multiplier = 10f;
#endif

        [Header("Animation Parameters")]
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private AvatarMask playerDanceAvatarMask;
        [SerializeField] private int playerDanceLayerIndex;
        [SerializeField] private float animationLayerWeightUpdateDelay = 0.1f;

        [Header("Requirements")]
        [SerializeField] private List<Pair<float, List<AvatarMaskBodyPart>>> avatarMaskBodyPartActivationRequirementsList;
        [SerializeField] private List<Pair<float, float>> layerWeightRequirementsList;

        public static event Action<float> OnDanceMoveMasteryUpdated;

        private float totalDanceMovesMastery = 0f;
        private int indexOfDanceMoveBeingPerformed;

        private void Awake()
        {
            totalDanceMovesMastery = PlayerPrefsManager.GetTotalDanceMovesMastery();
        }

        private async void Start()
        {
            EmoteWheelUI.OnDoDance += EmoteWheelUI_OnDoDance;

            ButtonsHandler.OnStopDancing += ButtonsHandler_OnStopDancing;

            ResetAvatarMask();
            CheckIfAnyAvatarMaskRequirmentFulfilled();

            await Task.Delay((int)(animationLayerWeightUpdateDelay * 1000f));
            CheckIfAnyLayerWeightRequirementFulfilled();
        }

        private void ButtonsHandler_OnStopDancing()
        {
            float totalMasteryPointsIncrease = playerDanceMovesManager.GetDanceMoveSOFromIndex(indexOfDanceMoveBeingPerformed).DanceMoveMasteryWeight;

#if UNITY_EDITOR
            totalMasteryPointsIncrease *= multiplier;
#endif

            totalDanceMovesMastery += totalMasteryPointsIncrease;

            totalDanceMovesMastery = Mathf.Clamp(totalDanceMovesMastery, 0f, totalMasteryPoints);
            OnDanceMoveMasteryUpdated?.Invoke(totalDanceMovesMastery);

            CheckIfAnyAvatarMaskRequirmentFulfilled();
            CheckIfAnyLayerWeightRequirementFulfilled();
        }

        private void ResetAvatarMask()
        {
            List<AvatarMaskBodyPart> allAvatarMaskBodyPartsList = Enum.GetValues(typeof(AvatarMaskBodyPart)).Cast<AvatarMaskBodyPart>().ToList();

            // Reset all the body parts first, because avatar mask is an asset and all the changes done at runtime in an asset gets saved to the asset database
            foreach (AvatarMaskBodyPart avatarMaskBodyPart in allAvatarMaskBodyPartsList)
            {
                playerDanceAvatarMask.SetHumanoidBodyPartActive(avatarMaskBodyPart, false);
            }

            // Then set the default active parts
            foreach(AvatarMaskBodyPart avatarMaskBodyPart in defaultActiveBodyPartsList)
            {
                playerDanceAvatarMask.SetHumanoidBodyPartActive(avatarMaskBodyPart, true);
            }
        }

        private void EmoteWheelUI_OnDoDance(int danceMoveIndex)
        {
            indexOfDanceMoveBeingPerformed = danceMoveIndex;
        }

        private void CheckIfAnyAvatarMaskRequirmentFulfilled()
        {
            foreach(var pair in avatarMaskBodyPartActivationRequirementsList)
            {
                if(totalDanceMovesMastery >= pair.first)
                {
                    foreach (AvatarMaskBodyPart avatarMaskBodyPart in pair.second)
                    {
                        playerDanceAvatarMask.SetHumanoidBodyPartActive(avatarMaskBodyPart, true);
                    }
                }
            }
        }

        private async void CheckIfAnyLayerWeightRequirementFulfilled()
        {
            for(int i = layerWeightRequirementsList.Count - 1; i >= 0; i--)
            {
                var pair = layerWeightRequirementsList[i];  

                if (totalDanceMovesMastery >= pair.first)
                {
                    await Task.Delay((int)(animationLayerWeightUpdateDelay * 1000f));
                    playerAnimator.SetLayerWeight(playerDanceLayerIndex, pair.second);

                    break;
                }
            }
        }

        private void OnDestroy()
        {
            EmoteWheelUI.OnDoDance -= EmoteWheelUI_OnDoDance;

            ButtonsHandler.OnStopDancing -= ButtonsHandler_OnStopDancing;
        }

        public float GetTotalDanceMovesMasteryNormalized()
        {
            return totalDanceMovesMastery / totalMasteryPoints;
        }
    }
}