using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class CinematicCameraNavigationUI : MonoBehaviour
    {
        [SerializeField] private CameraSwitchManager cameraSwitchManager;

        [Header("Objects to Hide")]
        [SerializeField] private List<GameObject> objectsToHideInCinematicMode;
        [SerializeField] private float fadeSpeed = 3f;

        [Header("UI Objects")]
        [SerializeField] private GameObject uiContainer;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        public static event Action OnCloseCinematicCamera;
        public static event Action OnOpenCinematicCamera;

        private Dictionary<GameObject, Coroutine> gameObjectCoroutinesMap;
        private Coroutine newCoroutine;

        private void Awake()
        {
            gameObjectCoroutinesMap = new Dictionary<GameObject, Coroutine>();  
        }

        private void Start()
        {
            openButton.onClick.AddListener(() =>
            {
                OnOpenCinematicCamera?.Invoke();
            });

            previousButton.onClick.AddListener(() =>
            {
                cameraSwitchManager.SwitchToNewCinematicCamera(false);
            });

            nextButton.onClick.AddListener(() =>
            {
                cameraSwitchManager.SwitchToNewCinematicCamera(true);
            });

            closeButton.onClick.AddListener(() =>
            {
                OnCloseCinematicCamera?.Invoke();
                ToggleUIContainerVisibility(false);
            });
        }

        private void ToggleVisibilityOfObjectsInCinematicMode(bool active)
        {
            foreach(GameObject gameObject in objectsToHideInCinematicMode)
            {
                CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();

                if (gameObjectCoroutinesMap.ContainsKey(gameObject))
                {
                    StopCoroutine(gameObjectCoroutinesMap[gameObject]);
                    newCoroutine = StartCoroutine(SlowlyMoveCanvasGroupAlpha(canvasGroup, active ? 1f : 0f));
                    gameObjectCoroutinesMap[gameObject] = newCoroutine;
                }
                else
                {
                    newCoroutine = StartCoroutine(SlowlyMoveCanvasGroupAlpha(canvasGroup, active ? 1f : 0f));
                    gameObjectCoroutinesMap.Add(gameObject, newCoroutine);
                }
            }
        }

        private IEnumerator SlowlyMoveCanvasGroupAlpha(CanvasGroup canvasGroup, float targetValue)
        {
            while(Mathf.Abs(canvasGroup.alpha - targetValue) >= 0.001f)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetValue, fadeSpeed * Time.deltaTime);

                yield return null;
            }
        }

        public void ToggleUIContainerVisibility(bool active)
        {
            uiContainer.SetActive(active);
            uiContainer.SetActive(active);

            ToggleVisibilityOfObjectsInCinematicMode(!active);
        }
    }
}
