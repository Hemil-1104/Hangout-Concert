using System;
using UnityEngine;
using Debug = MyGames.Debug;

public static class PlayerAnimatorKeys
{
    public static string Is_Moving = "Is_Moving";
    public static string Is_Dancing = "Is_Dancing";
    public static string Dance_Emote_Index = "Dance_Emote_Index";
}

namespace nostra.booboogames.hangoutconcert
{
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerController playerController;
        private Animator animator;

        public static event Action OnStartedDancing;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            EmoteWheelUI.OnDoDance += EmoteWheelUI_OnDoDance;

            ButtonsHandler.OnStopDancing += ButtonsHandler_OnStopDancing;
        }

        private void ButtonsHandler_OnStopDancing()
        {
            animator.SetBool(PlayerAnimatorKeys.Is_Dancing, false);
            animator.SetInteger(PlayerAnimatorKeys.Dance_Emote_Index, -1);
        }

        private void EmoteWheelUI_OnDoDance(int danceEmoteIndex)
        {
            animator.SetBool(PlayerAnimatorKeys.Is_Moving, false);
            animator.SetBool(PlayerAnimatorKeys.Is_Dancing, true);
            animator.SetInteger(PlayerAnimatorKeys.Dance_Emote_Index, danceEmoteIndex);

            OnStartedDancing?.Invoke();
        }

        private void Update()
        {
            animator.SetBool(PlayerAnimatorKeys.Is_Moving, playerController.IsMoving());
        }

        private void OnDestroy()
        {
            EmoteWheelUI.OnDoDance -= EmoteWheelUI_OnDoDance;

            ButtonsHandler.OnStopDancing -= ButtonsHandler_OnStopDancing;
        }
    }
}