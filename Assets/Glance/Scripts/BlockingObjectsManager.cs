using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class BlockingObjectsManager : MonoBehaviour
    {
        private const string BASE_COLOR = "_BaseColor";

        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform target;
        [SerializeField] private Camera mainCamera;
        [SerializeField][Range(0, 1f)] private float fadedAlpha = 0.33f;
        [SerializeField] private Vector3 targetPositionOffset = Vector3.up;
        [SerializeField] private float fadeSpeed = 1;

        [Header("Read Only Data")]
        [SerializeField] private List<FadingObject> objectsBlockingView = new List<FadingObject>();

        private Dictionary<FadingObject, Coroutine> runningCoroutines = new Dictionary<FadingObject, Coroutine>();

        private RaycastHit[] hits = new RaycastHit[10];

        private void OnEnable()
        {
            StartCoroutine(CheckForObjects());
        }

        private IEnumerator CheckForObjects()
        {
            while (true)
            {
                int hits = Physics.RaycastNonAlloc(
                    mainCamera.transform.position,
                    (target.transform.position + targetPositionOffset - mainCamera.transform.position).normalized,
                    this.hits,
                    Vector3.Distance(mainCamera.transform.position, target.transform.position + targetPositionOffset),
                    layerMask
                );

                if (hits > 0)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        FadingObject fadingObject = GetFadingObjectFromHit(this.hits[i]);

                        if (fadingObject != null && !objectsBlockingView.Contains(fadingObject))
                        {
                            if (runningCoroutines.ContainsKey(fadingObject))
                            {
                                if (runningCoroutines[fadingObject] != null)
                                {
                                    StopCoroutine(runningCoroutines[fadingObject]);
                                }

                                runningCoroutines.Remove(fadingObject);
                            }

                            runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                            objectsBlockingView.Add(fadingObject);
                        }
                    }
                }

                FadeInObjectsNoLongerBeingHit();

                ClearHits();

                yield return null;
            }
        }

        private void FadeInObjectsNoLongerBeingHit()
        {
            List<FadingObject> objectsToRemove = new List<FadingObject>(objectsBlockingView.Count);

            foreach (FadingObject fadingObject in objectsBlockingView)
            {
                bool objectIsBeingHit = false;
                for (int i = 0; i < hits.Length; i++)
                {
                    FadingObject hitFadingObject = GetFadingObjectFromHit(hits[i]);
                    if (hitFadingObject != null && fadingObject == hitFadingObject)
                    {
                        objectIsBeingHit = true;
                        break;
                    }
                }

                if (!objectIsBeingHit)
                {
                    if (runningCoroutines.ContainsKey(fadingObject))
                    {
                        if (runningCoroutines[fadingObject] != null)
                        {
                            StopCoroutine(runningCoroutines[fadingObject]);
                        }
                        runningCoroutines.Remove(fadingObject);
                    }

                    runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectIn(fadingObject)));
                    objectsToRemove.Add(fadingObject);
                }
            }

            foreach (FadingObject removeObject in objectsToRemove)
            {
                objectsBlockingView.Remove(removeObject);
            }
        }

        private IEnumerator FadeObjectOut(FadingObject fadingObject)
        {
            Renderer objectRenderer = fadingObject._renderer;

            objectRenderer.material = fadingObject.transparentMaterial;
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            objectRenderer.GetPropertyBlock(materialPropertyBlock);

            float time = 0;
            Color baseColor = objectRenderer.material.GetColor(BASE_COLOR);

            while (baseColor.a > fadedAlpha)
            {
                baseColor = new Color(
                        baseColor.r,
                        baseColor.g,
                        baseColor.b,
                        Mathf.Lerp(fadingObject.initialAlpha, fadedAlpha, time * fadeSpeed));

                materialPropertyBlock.SetColor(BASE_COLOR, baseColor);
                objectRenderer.SetPropertyBlock(materialPropertyBlock);

                time += Time.deltaTime;
                yield return null;
            }

            if (runningCoroutines.ContainsKey(fadingObject))
            {
                StopCoroutine(runningCoroutines[fadingObject]);
                runningCoroutines.Remove(fadingObject);
            }
        }

        private IEnumerator FadeObjectIn(FadingObject fadingObject)
        {
            Renderer objectRenderer = fadingObject._renderer;

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            if (objectRenderer == null)
            {
                Debug.Log(fadingObject.gameObject.name);
            }
            objectRenderer.GetPropertyBlock(materialPropertyBlock);

            float time = 0;
            Color baseColor = materialPropertyBlock.GetColor(BASE_COLOR);

            while (baseColor.a < fadingObject.initialAlpha)
            {
                baseColor = new Color(
                        baseColor.r,
                        baseColor.g,
                        baseColor.b,
                        Mathf.Lerp(fadedAlpha, fadingObject.initialAlpha, time * fadeSpeed));

                materialPropertyBlock.SetColor(BASE_COLOR, baseColor);
                objectRenderer.SetPropertyBlock(materialPropertyBlock);

                time += Time.deltaTime;
                yield return null;
            }

            objectRenderer.material = fadingObject.originalMaterial;

            if (runningCoroutines.ContainsKey(fadingObject))
            {
                StopCoroutine(runningCoroutines[fadingObject]);
                runningCoroutines.Remove(fadingObject);
            }
        }

        private void ClearHits()
        {
            System.Array.Clear(hits, 0, hits.Length);
        }

        private FadingObject GetFadingObjectFromHit(RaycastHit Hit)
        {
            return Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;
        }
    }
}