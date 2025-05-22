using System.Collections.Generic;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class CharacterVisualsManager : MonoBehaviour
    {
        [SerializeField] private List<CharacterVisual> characterVisualsList;

        [SerializeField] private List<GameObject> hairStylesList;

        private void Awake()
        {
            GiveRandomClothesToBots();
        }

        private void GiveRandomClothesToBots()
        {
            foreach (CharacterVisual botVisual in characterVisualsList)
            {
                GiveRandomClothesToBot(botVisual);
            }
        }

        private void SetRandomClothFromTransform(Transform clothesContainer)
        {
            foreach (Transform child in clothesContainer)
            {
                child.gameObject.SetActive(false);
            }

            Transform randomCloth = GetRandomClothFromTransform(clothesContainer);
            randomCloth.gameObject.SetActive(true);
        }

        private Transform GetRandomClothFromTransform(Transform clothesContainer)
        {
            return clothesContainer.GetChild(Random.Range(0, clothesContainer.childCount));
        }

        private GameObject GetRandomHairStylePrefabFromList()
        {
            return hairStylesList[Random.Range(0, hairStylesList.Count)];
        }

        public void GiveRandomClothesToBot(CharacterVisual botVisual)
        {
            Transform headTransform = botVisual.GetHeadTransform();
            Transform topsContainer = botVisual.GetTopsContainer();
            Transform pantsContainer = botVisual.GetPantsContainer();
            Transform shoesContainer = botVisual.GetShoesContainer();

            SetRandomClothFromTransform(topsContainer);
            SetRandomClothFromTransform(pantsContainer);
            SetRandomClothFromTransform(shoesContainer);

            GameObject hairStylePrefab = GetRandomHairStylePrefabFromList();

            GameObject hairStyleObject = Instantiate(hairStylePrefab, headTransform);
            hairStyleObject.transform.localScale = Vector3.one * 1.1f;

            botVisual.SetRenderer();
        }

        public void AddToCharacterVisualsList(CharacterVisual characterVisual)
        {
            characterVisualsList.Add(characterVisual);
            GiveRandomClothesToBot(characterVisual);
        }

        public List<CharacterVisual> GetCharacterVisualsList()
        {
            return characterVisualsList;
        }
    }
}