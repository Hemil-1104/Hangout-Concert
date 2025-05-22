using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using Debug = MyGames.Debug;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace nostra.booboogames.hangoutconcert
{
    public class FloatingJoystick : MonoBehaviour
    {
        [SerializeField] private FloatingJoystick otherFloatingJoystick;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private Vector2 joystickSize = new Vector2(300f, 300f);
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform knob;
        [SerializeField] private List<Transform> inputBlockingUIScreensList;
        [SerializeField] private bool isCameraJoystick;

        [Header("Joystick Constraint Parameters")]
        [SerializeField] private float normalizedYThreshold = 0.3f;

        private Vector2 screenPositionOffset;
        private Finger movementFinger;
        private Vector2 movementAmount;
        private PointerEventData clickData;
        private List<RaycastResult> raycastResults;

        private void Awake()
        {
            screenPositionOffset = new Vector2(Screen.width / 2f, joystickSize.y / 2f);

            clickData = new PointerEventData(EventSystem.current);
            raycastResults = new List<RaycastResult>();
        }

        private void Start()
        {
            InputManager.OnTouchedFinger += HandleFingerDown;
            InputManager.OnMovedFinger += HandleMoveFinger;
            InputManager.OnLostFinger += HandleLoseFinger;
        }

        private void HandleLoseFinger(Finger lostFinger)
        {
            if (lostFinger == movementFinger)
            {
                movementFinger = null;

                movementAmount = Vector2.zero;
                background.gameObject.SetActive(false);
                knob.anchoredPosition = Vector2.zero;
            }
        }

        private void HandleMoveFinger(Finger movedFinger)
        {
            if (movementFinger == movedFinger)
            {
                Vector2 knobPosition;

                // maxMovement is for restricting the knob movement to a certain point, so that it doesn't go beyond the background
                float maxMovement = joystickSize.x / 2f;
                ETouch.Touch currentTouch = movedFinger.currentTouch;
                Vector2 touchPositionWithOffset = currentTouch.screenPosition - screenPositionOffset;

                // If the player drags the knob way higher than its maximum movement, then we clamp the knob position to not go beyond maxMovement
                if (Vector2.Distance(touchPositionWithOffset, background.anchoredPosition) > maxMovement)
                {
                    knobPosition = (touchPositionWithOffset - background.anchoredPosition).normalized * maxMovement;
                }
                // else just take the distance between touch position and the parent position
                else
                {
                    knobPosition = touchPositionWithOffset - background.anchoredPosition;
                }

                knob.anchoredPosition = knobPosition;

                // To normalize the movement amount between -1 and 1 
                movementAmount = knobPosition / maxMovement;
            }
        }

        private void HandleFingerDown(Finger touchedFinger)
        {
            if (IsAnyInputBlockingUIScreenActive() || movementFinger != null || !IsValidStartingPosition(touchedFinger.screenPosition) || HasClickedOverUI(touchedFinger.screenPosition) || otherFloatingJoystick.GetJoystickFinger() == touchedFinger) return;

            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;

            if (!isCameraJoystick)
            {
                background.gameObject.SetActive(true);
            }

            background.sizeDelta = joystickSize;
            background.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }

        /// <summary>
        /// Clamps the start position in a way that the main joystick doesn't go out of bounds (as the pivot of the joystick will be in center)
        /// </summary>
        /// <param name="startPosition"></param>
        /// <returns></returns>
        private Vector2 ClampStartPosition(Vector2 startPosition)
        {
            if (startPosition.x < joystickSize.x / 2f)
            {
                startPosition.x = joystickSize.x / 2f;
            }

            if (startPosition.y < joystickSize.y / 2f)
            {
                startPosition.y = joystickSize.y / 2f;
            }
            else if (startPosition.y > Screen.height - joystickSize.y / 2f)
            {
                startPosition.y = Screen.height - joystickSize.y / 2f;
            }

            return startPosition - screenPositionOffset;
        }

        /// <summary>
        /// (Can Change the Valid position condition according to the game requirements)
        /// Here, the touch is considered a valid position, if the touch y position is less than 30% of the screen (eg. if screen height is 2160 pixels, then it will be considered a valid touch, if the y position is < 648)
        /// </summary>
        /// <returns></returns>
        private bool IsValidStartingPosition(Vector2 startPosition)
        {
            float height = Screen.height;
            float mouseYPosition = startPosition.y;
            float mouseYPositionNormalized = mouseYPosition / height;
            bool isValidPosition = isCameraJoystick ? mouseYPositionNormalized >= normalizedYThreshold : mouseYPositionNormalized >= 0f && mouseYPositionNormalized < normalizedYThreshold;

            return isValidPosition;
        }

        private bool IsAnyInputBlockingUIScreenActive()
        {
            foreach (Transform inputBlockingUIScreen in inputBlockingUIScreensList)
            {
                if (inputBlockingUIScreen.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasClickedOverUI(Vector2 mousePosition)
        {
            clickData.position = mousePosition;
            raycastResults.Clear();
            graphicRaycaster.Raycast(clickData, raycastResults);

            if (raycastResults.Count == 1 && raycastResults[0].gameObject == gameObject) return false;

            foreach (RaycastResult raycastResult in raycastResults)
            {
                //Debug.Log($"Clicked on UI Object {raycastResult.gameObject.name}");
            }

            return raycastResults.Count > 0;
        }

        private void OnDestroy()
        {
            InputManager.OnTouchedFinger -= HandleFingerDown;
            InputManager.OnMovedFinger -= HandleMoveFinger;
            InputManager.OnLostFinger -= HandleLoseFinger;
        }

        public Finger GetJoystickFinger()
        {
            return movementFinger;
        }

        public Vector2 GetMovementAmount()
        {
            return movementAmount;
        }
    }
}