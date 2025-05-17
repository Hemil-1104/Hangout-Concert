using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class InteractableCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject interactionContainer;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            //transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }

        public void ShowInteractionContainer()
        {
            interactionContainer.SetActive(true);
        }

        public void HideInteractionContainer()
        {
            interactionContainer.SetActive(false);
        }
    }
}