using System.Collections.Generic;
using MyGames;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class NPCManager : MonoBehaviour
    {
        public static int Count { get; private set; }

        [SerializeField] private CharacterVisualsManager characterVisualsManager;
        [SerializeField] private GameObject spawnBoundary;
        [SerializeField] private NPC npcPrefab;
        [SerializeField] private Transform npcContainer;
        [SerializeField] private int numberOfNpcsToSpawn;

        private List<NPC> npcList;

        private void Awake()
        {
            npcList = new List<NPC>();

            SpawnNpcs();
        }

        private void SpawnNpcs()
        {
            for(int i = 0; i < numberOfNpcsToSpawn; i++)
            {
                Vector3 spawnPosition = Utils.GetRandomPositionInObjectGeometry(spawnBoundary);

                NPC npc = Instantiate(npcPrefab, spawnPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), npcContainer);
                CharacterVisual npcVisual = npc.GetComponent<CharacterVisual>();

                npc.gameObject.name = $"NPC_{Count++}";
                npcList.Add(npc);
                characterVisualsManager.AddToCharacterVisualsList(npcVisual);
            }
        }
    }
}