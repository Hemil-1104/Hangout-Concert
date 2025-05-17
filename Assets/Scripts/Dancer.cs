using System;
using System.Collections.Generic;
using MyGames;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public static class DancerAnimatorKeys
    {
        public static string Dance_Emote_Index = "Dance_Emote_Index";
    }

    public class Dancer : MonoBehaviour
    {
        [SerializeField] private List<DanceMoveSO> danceClips;
        [SerializeField] private List<GameObject> clickableObjectsList;
        [SerializeField] private int danceMoveStartIndex;
        [SerializeField] private int danceMoveEndIndex;

        public static event Action OnTryingToScanAlreadyUnlockedMove;
        public static event Action<Transform> OnScanDancer;

        private PlayerDanceMovesManager playerDanceMovesManager;
        private Animator animator;
        private Camera mainCamera;
        private int currentDanceEmoteIndex;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            mainCamera = Camera.main;

            DoRandomDance();
        }

        private void Start()
        {
            playerDanceMovesManager = DependencyManager.PlayerDanceMovesManager;
        }

        private void Update()
        {
            HandlePlayerInteraction();
        }

        private void HandlePlayerInteraction()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && DidTouchAnyClickableObject(hit.collider.gameObject))
            {
                if(playerDanceMovesManager.IsDanceMoveAlreadyUnlocked(currentDanceEmoteIndex))
                {
                    OnTryingToScanAlreadyUnlockedMove?.Invoke();
                }
                else
                {
                    OnScanDancer?.Invoke(transform);
                }
            }
        }

        private void DoRandomDance()
        {
            int randomIndex = Utils.GetRandomIndexFromList(danceClips, danceMoveStartIndex, danceMoveEndIndex);

            currentDanceEmoteIndex = randomIndex;
            animator.SetInteger(DancerAnimatorKeys.Dance_Emote_Index, randomIndex);
        }

        private bool DidTouchAnyClickableObject(GameObject colliderObject)
        {
            foreach (GameObject clickableObject in clickableObjectsList)
            {
                if (colliderObject == clickableObject)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetCurrentDanceEmoteIndex()
        {
            return currentDanceEmoteIndex;
        }
    }
}