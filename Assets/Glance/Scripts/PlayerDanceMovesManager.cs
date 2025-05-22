using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class PlayerDanceMovesManager : MonoBehaviour
    {
        [SerializeField] private List<DanceMoveSO> danceMoveSOsList;
        [SerializeField] private List<DanceMoveSO> starterDanceMovesList;

        public static event Action<DanceMoveSO> OnNewDanceMoveUnlocked;

        private List<DanceMoveSO> unlockedDanceMovesList;

        private void Awake()
        {
            unlockedDanceMovesList = new List<DanceMoveSO>();

            foreach(DanceMoveSO danceMoveSO in starterDanceMovesList)
            {
                unlockedDanceMovesList.Add(danceMoveSO);
            }
        }

        private void Start()
        {
            DanceMoveScanUI.OnLearnedDanceMove += DanceMoveScanUI_OnLearnedDanceMove;

            DanceMovesShopUI.OnPurchaseDanceMove += DanceMovesShopUI_OnPurchaseDanceMove;
        }

        private void DanceMovesShopUI_OnPurchaseDanceMove(int danceMoveIndex)
        {
            UnlockDanceMove(danceMoveIndex);
        }

        private void DanceMoveScanUI_OnLearnedDanceMove(int danceMoveIndex)
        {
            UnlockDanceMove(danceMoveIndex);
        }

        private void UnlockDanceMove(int danceMoveIndex)
        {
            DanceMoveSO danceMoveSO = GetDanceMoveSOFromIndex(danceMoveIndex);

            if (IsDanceMoveAlreadyUnlocked(danceMoveSO)) return;

            unlockedDanceMovesList.Add(danceMoveSO);
            OnNewDanceMoveUnlocked?.Invoke(danceMoveSO);
        }

        private bool IsDanceMoveAlreadyUnlocked(DanceMoveSO danceMoveSO)
        {
            return unlockedDanceMovesList.Contains(danceMoveSO);
        }

        public bool IsDanceMoveAlreadyUnlocked(int danceMoveIndex)
        {
            DanceMoveSO danceMoveSO = GetDanceMoveSOFromIndex(danceMoveIndex);
            return IsDanceMoveAlreadyUnlocked(danceMoveSO);
        }

        public List<DanceMoveSO> GetUnlockedDanceMovesList()
        {
            return unlockedDanceMovesList;
        }

        public DanceMoveSO GetDanceMoveSOFromIndex(int danceMoveIndex)
        {
            foreach (DanceMoveSO danceMoveSO in danceMoveSOsList)
            {
                if (danceMoveSO.DanceMoveIndex == danceMoveIndex)
                {
                    return danceMoveSO;
                }
            }

            return null;
        }

        private void OnDestroy()
        {
            DanceMoveScanUI.OnLearnedDanceMove -= DanceMoveScanUI_OnLearnedDanceMove;

            DanceMovesShopUI.OnPurchaseDanceMove -= DanceMovesShopUI_OnPurchaseDanceMove;
        }
    }
}
