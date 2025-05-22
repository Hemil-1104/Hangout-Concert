using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class PhotoModeCameraHandler : MonoBehaviour
    {
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
        }
    }
}