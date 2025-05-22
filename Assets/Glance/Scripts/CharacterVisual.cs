using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class CharacterVisual : MonoBehaviour
    {
        [SerializeField] private Transform headTransform;

        [Header("Clothes Parents")]
        [SerializeField] private Transform topsContainer;
        [SerializeField] private Transform pantsContainer;
        [SerializeField] private Transform shoesContainer;

        private Renderer[] renderersArray = new Renderer[10];
        private Renderer characterRenderer;

        public void SetRenderer()
        {
            renderersArray = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderersArray)
            {
                if (renderer.gameObject.activeInHierarchy)
                {
                    characterRenderer = renderer;
                    break;
                }
            }
        }

        public Transform GetHeadTransform()
        {
            return headTransform;
        }

        public Transform GetTopsContainer()
        {
            return topsContainer;
        }

        public Transform GetPantsContainer()
        {
            return pantsContainer;
        }

        public Transform GetShoesContainer()
        {
            return shoesContainer;
        }

        public Renderer GetCharacterRenderer()
        {
            return characterRenderer;
        }
    }
}