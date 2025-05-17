using System;
using System.Collections;
using System.Collections.Generic;
using MyGames;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DJBoothMinigameManager : XPEarningMethod
    {
        public enum ScoreRating
        {
            Good,
            Great,
            Awesome,
            Perfect
        }

        [Header("Game Tiles Parameters")]
        [SerializeField] private DJBoothGameTileObjectPool djBoothGameTileObjectPool;

        [Header("Game Parameters")]
        [SerializeField] private List<RectTransform> tileSpawnPositionsList;
        [SerializeField] private List<Pair<float, ScoreRating>> scoreRatingThresholdsList;
        [SerializeField] private float minimumTileSpawnDelay;
        [SerializeField] private float maximumTileSpawnDelay;
        [SerializeField] private int numberOfTilesToSpawn = 50;

        public static event Action OnATileDone;
        public static event Action<int> OnPointsChanged;

        private List<GameObject> leftSideTilesList;
        private List<GameObject> rightSideTilesList;
        private List<GameObject> centerTilesList;

        public static int NumberOfTilesToSpawn { get; private set; }
        public static int NumberOfTilesSpawned { get; private set; }
        public static int NumberOfTilesDone { get; private set; }
        public static int TotalPointsPossible {  get; private set; }
        public static int Points { get; private set; }

        private int maximumXp;
        private List<GameObject> tilesListToCheck;

        private void Awake()
        {
            leftSideTilesList = new List<GameObject>();
            rightSideTilesList = new List<GameObject>();
            centerTilesList = new List<GameObject>();

            NumberOfTilesToSpawn = numberOfTilesToSpawn;
        }

        protected override void Start()
        {
            DJBoothGameTile.OnTileOutOfBounds += DJBoothGameTile_OnTileOutOfBounds;

            DJBoothMinigameUI.OnGameClosed += DJBoothMinigameUI_OnGameClosed;

            DJBooth.OnStartedPlayingDJBoothGame += DJBooth_OnStartedPlayingDJBoothGame;

            maximumXp = GameManager.DJBoothMinigameMaximumXp;
            xpEarningCooldown = GameManager.DJBoothMinigameXpEarningCooldown;

            base.Start();
        }

        private void DJBooth_OnStartedPlayingDJBoothGame()
        {
            StartCoroutine(StartGame());
        }

        private void DJBoothMinigameUI_OnGameClosed()
        {
            ResetGame();
            StopAllCoroutines();
        }

        private void DJBoothGameTile_OnTileOutOfBounds(GameObject tile)
        {
            if(leftSideTilesList.Contains(tile))
            {
                leftSideTilesList.Remove(tile);
            }
            else if(rightSideTilesList.Contains(tile))
            {
                rightSideTilesList.Remove(tile);
            }
            else
            {
                centerTilesList.Remove(tile);
            }

            TotalPointsPossible += tile.GetComponent<DJBoothGameTile>().TotalPoints;
            NumberOfTilesDone += 1;

            OnATileDone?.Invoke();
        }

        public void HandleButtonClick(RectTransform buttonRectTransform, int buttonIndex)
        {
            tilesListToCheck = GetTilesListAccordingToIndex(buttonIndex);

            if (tilesListToCheck.Count == 0) return;

            float threshold = buttonRectTransform.anchoredPosition.y + buttonRectTransform.rect.height / 2f;

            GameObject tileObject = tilesListToCheck[0];
            RectTransform firstTileRectTransform = tileObject.GetComponent<RectTransform>();

            float difference = Mathf.Abs(firstTileRectTransform.anchoredPosition.y - buttonRectTransform.anchoredPosition.y);
            float normalizedDifference = difference / threshold;

            if (normalizedDifference < 1f)
            {
                for (int i = 1; i < scoreRatingThresholdsList.Count; i++)
                {
                    if(normalizedDifference >= scoreRatingThresholdsList[i].first && normalizedDifference < scoreRatingThresholdsList[i - 1].first)
                    {
                        //Debug.Log(scoreRatingThresholdsList[i - 1].second);
                    }
                }

                if(normalizedDifference >= 0f && normalizedDifference < scoreRatingThresholdsList[^1].first)
                {
                    //Debug.Log(scoreRatingThresholdsList[^1].second);
                }

                tileObject.GetComponent<DJBoothGameTile>().OnMatched(buttonRectTransform.anchoredPosition);
                AddPoints(tileObject, normalizedDifference);
                TotalPointsPossible += tileObject.GetComponent<DJBoothGameTile>().TotalPoints;
                tilesListToCheck.RemoveAt(0);

                NumberOfTilesDone += 1;
                OnATileDone?.Invoke();
            }
        }

        private void ResetGame()
        {
            Points = 0;
            NumberOfTilesDone = 0;
            NumberOfTilesSpawned = 0;

            OnPointsChanged?.Invoke(Points);
        }

        private void AddPoints(GameObject tile, float normalizedDifference)
        {
            DJBoothGameTile djBoothGameTile = tile.GetComponent<DJBoothGameTile>();
            int totalPoints = djBoothGameTile.TotalPoints;

            Points += totalPoints - Mathf.RoundToInt(normalizedDifference * totalPoints) / 2;

            OnPointsChanged?.Invoke(Points);
        }

        private IEnumerator StartGame()
        {
            while(NumberOfTilesSpawned < numberOfTilesToSpawn)
            {
                int tileSpawnPositionIndex = Utils.GetRandomIndexFromList(tileSpawnPositionsList);

                GameObject tile = djBoothGameTileObjectPool.Get().gameObject;
                NumberOfTilesSpawned += 1;

                RectTransform rectTransform = tile.GetComponent<RectTransform>();

                SetupTileAccordingToIndex(tileSpawnPositionIndex, rectTransform);

                float spawnDelay = Utils.GetRandomValueBetween(minimumTileSpawnDelay, maximumTileSpawnDelay);
                yield return new WaitForSeconds(spawnDelay);
            }

            HandleGameFinish();
        }

        private void HandleGameFinish()
        {
            if (canEarnXp)
            {
                int totalXpEarned = maximumXp * Mathf.RoundToInt(Points / (float)TotalPointsPossible);

                EarnXp(totalXpEarned);

                StartCoroutine(StartCooldown());
            }
        }

        private List<GameObject> GetTilesListAccordingToIndex(int index)
        {
            if (index == 0)
            {
                return leftSideTilesList;
            }
            else if(index == 1)
            {
                return centerTilesList;
            }
            else
            {
                return rightSideTilesList;
            }
        }

        private void SetupTileAccordingToIndex(int index, RectTransform tileRectTransform)
        {
            GameObject tile = tileRectTransform.gameObject;

            RectTransform refTransform = tileSpawnPositionsList[index];
            tileRectTransform.anchorMin = refTransform.anchorMin;
            tileRectTransform.anchorMax = refTransform.anchorMax;
            tileRectTransform.pivot = refTransform.pivot;
            tileRectTransform.position = refTransform.position;

            if (index == 0)
            {
                leftSideTilesList.Add(tile);
                tile.name = $"Left_Tile_{leftSideTilesList.Count}";
            }
            else if(index == 1)
            {
                centerTilesList.Add(tile);
                tile.name = $"Center_Tile_{centerTilesList.Count}";
            }
            else
            {
                rightSideTilesList.Add(tile);
                tile.name = $"Right_Tile_{rightSideTilesList.Count}";
            }

            tile.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            DJBoothGameTile.OnTileOutOfBounds -= DJBoothGameTile_OnTileOutOfBounds;

            DJBoothMinigameUI.OnGameClosed -= DJBoothMinigameUI_OnGameClosed;

            DJBooth.OnStartedPlayingDJBoothGame -= DJBooth_OnStartedPlayingDJBoothGame;
        }
    }
}