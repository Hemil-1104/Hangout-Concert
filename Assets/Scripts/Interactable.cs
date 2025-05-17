using System.Collections.Generic;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    [RequireComponent(typeof(Outline))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private float interactionRadius;
        [SerializeField] private List<GameObject> clickableObjectsList;
        [SerializeField] private InteractableCanvas interactableCanvas;

        protected bool isPlayerInsideProximity = false;
        protected bool isPlayerInteracting = false;

        private Outline outline;
        private Collider[] colliders = new Collider[20];
        private Camera mainCamera;

        protected virtual void Awake()
        {
            mainCamera = Camera.main;

            outline = GetComponent<Outline>();
        }

        protected virtual void Start()
        {
            outline.enabled = false;
        }

        protected virtual void Update()
        {
            CheckForPlayerProximity();
            CheckForPlayerTouch();
        }

        private void CheckForPlayerProximity()
        {
            int totalColliders = Physics.OverlapSphereNonAlloc(transform.position, interactionRadius, colliders);
            for (int i = 0; i < totalColliders; i++)
            {
                if (colliders[i].TryGetComponent(out Player player))
                {
                    if (isPlayerInsideProximity)
                    {
                        return;
                    }

                    isPlayerInsideProximity = true;
                    HandlePlayerInsideProximity();

                    return;
                }
            }

            isPlayerInsideProximity = false;
            HandlePlayerOutsideProximity();
        }

        private void CheckForPlayerTouch()
        {
            if (!isPlayerInsideProximity || isPlayerInteracting) return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit raycastHit) && IsClickingOnValidObject(raycastHit.collider.gameObject))
                {
                    Interact();
                }
            }
        }

        private void HandlePlayerInsideProximity()
        {
            outline.enabled = true;

            interactableCanvas.ShowInteractionContainer();
        }

        private void HandlePlayerOutsideProximity()
        {
            outline.enabled = false;

            isPlayerInteracting = false;
            interactableCanvas.HideInteractionContainer();
        }

        private bool IsClickingOnValidObject(GameObject colliderObject)
        {
            foreach (GameObject clickableObject in clickableObjectsList)
            {
                if (colliderObject == clickableObject) return true;
            }

            return false;
        }

        public virtual void Interact() 
        { 
            isPlayerInteracting = true;
        }
    }
}