using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DJBoothGameTile : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float fadeSpeed = 2f;
        
        public int TotalPoints = 100;
        public static event Action<GameObject> OnTileOutOfBounds;

        private DJBoothGameTileObjectPool djBoothGameTileObjectPool;
        private Image image;
        private RectTransform rectTransform;
        private float yLowerLimit;

        private void OnEnable()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

            StartCoroutine(StartMoving());
        }

        private void Awake()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();

            yLowerLimit = -rectTransform.rect.size.y - 100f;
        }

        private IEnumerator StartMoving()
        {
            while (true)
            {
                rectTransform.anchoredPosition -= new Vector2(0f, moveSpeed * Time.deltaTime);

                if (rectTransform.anchoredPosition.y <= yLowerLimit)
                {
                    OnTileOutOfBounds?.Invoke(gameObject);
                    StartCoroutine(FadeTile());
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator FadeTile()
        {
            while (image.color.a > 0.1f)
            {
                Color color = image.color;
                float alpha = Mathf.Lerp(color.a, 0f, fadeSpeed * Time.deltaTime);
                image.color = new Color(color.r, color.g, color.b, alpha);

                yield return null;
            }

            djBoothGameTileObjectPool.Release(this);
        }

        public void OnMatched(Vector2 matchAnchorPosition)
        {
            StopAllCoroutines();
            rectTransform.anchoredPosition = matchAnchorPosition;

            StartCoroutine(FadeTile());
        }

        public void Setup(DJBoothGameTileObjectPool djBoothGameTileObjectPool)
        {
            this.djBoothGameTileObjectPool = djBoothGameTileObjectPool;
        }
    }
}
