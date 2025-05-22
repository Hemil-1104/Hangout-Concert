using System.Collections.Generic;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DJBoothGameTileObjectPool : MonoBehaviour
    {
        [SerializeField] private DJBoothGameTile djBoothGameTilePrefab;
        [SerializeField] private Transform djBoothGameTilesContainer;
        [SerializeField] private int initialObjects = 50;

        private List<DJBoothGameTile> djBoothGameTilesList;

        private void Awake()
        {
            djBoothGameTilesList = new List<DJBoothGameTile>();

            AddInitialObjects();
        }

        private void AddInitialObjects()
        {
            for(int i = 0; i < initialObjects; i++)
            {
                DJBoothGameTile djBoothGameTile = Instantiate(djBoothGameTilePrefab, djBoothGameTilesContainer);
                djBoothGameTile.Setup(this);

                djBoothGameTilesList.Add(djBoothGameTile);
                djBoothGameTile.gameObject.SetActive(false);
            }
        }

        public void Release(DJBoothGameTile djBoothGameTile)
        {
            djBoothGameTile.gameObject.SetActive(false);
            djBoothGameTilesList.Add(djBoothGameTile);
        }

        public DJBoothGameTile Get()
        {
            DJBoothGameTile djBoothGameTile = null;

            if (djBoothGameTilesList.Count > 0)
            {
                djBoothGameTile = djBoothGameTilesList[0];
                djBoothGameTilesList.RemoveAt(0);
            }
            else
            {
                djBoothGameTile = Instantiate(djBoothGameTilePrefab, djBoothGameTilesContainer);
                djBoothGameTile.Setup(this);
            }

            return djBoothGameTile;
        }
    }
}
