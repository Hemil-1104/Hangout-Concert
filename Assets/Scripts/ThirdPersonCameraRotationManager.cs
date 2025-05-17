using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class ThirdPersonCameraRotationManager : MonoBehaviour
    {
        [SerializeField] private float sensitivity;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform parentTransform;

        private void Update()
        {
            HandleCameraRotationInput();
        }

        private void HandleCameraRotationInput()
        {
            Vector2 lookVector = InputManager.GetNormalizedLookVector();
            parentTransform.localEulerAngles += new Vector3(0f, lookVector.x * sensitivity, 0f);
        }

        private void LateUpdate()
        {
            parentTransform.position = playerTransform.position;
        }

        [ContextMenu("Go To Player Position")]
        private void GoToPlayerPosition()
        {
            parentTransform.position = playerTransform.position;
            parentTransform.localEulerAngles = playerTransform.localEulerAngles;
        }
    }
}