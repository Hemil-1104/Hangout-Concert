using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using UnityEditor;
using System;

namespace nostra.booboogames.hangoutconcert
{
    public class InputManager : MonoBehaviour
    {
        public enum ControlScheme
        {
            Touch = 0,
            KBM = 1,
            Gamepad = 2
        }

        private const string GAMEPAD = "Gamepad";
        private const string KEYBOARD_MOUSE = "Keyboard&Mouse";

        [SerializeField] private FloatingJoystick movementFloatingJoystick;
        [SerializeField] private FloatingJoystick cameraFloatingJoystick;

        public static event Action<Finger> OnTouchedFinger;
        public static event Action<Finger> OnMovedFinger;
        public static event Action<Finger> OnLostFinger;

        private static PlayerInputActions inputActions;
        private static FloatingJoystick m_CameraFloatingJoystick;
        private static FloatingJoystick m_MovementFloatingJoystick;
        private static ControlScheme currentControlScheme;

        private float screenWidth;
        private float screenHeight;
        private PlayerInput playerInput;

        private void Awake()
        {
            inputActions = new PlayerInputActions();

            m_CameraFloatingJoystick = cameraFloatingJoystick;
            m_MovementFloatingJoystick = movementFloatingJoystick;

            playerInput = GetComponent<PlayerInput>();
            currentControlScheme = ControlScheme.Touch;

            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }

        private void OnEnable()
        {
            inputActions.Enable();
            EnhancedTouchSupport.Enable();

            ETouch.Touch.onFingerDown += Touch_OnFingerDown;
            ETouch.Touch.onFingerMove += Touch_OnFingerMove;
            ETouch.Touch.onFingerUp += Touch_OnFingerUp;
        }

        private void Touch_OnFingerUp(Finger lostFinger)
        {
            OnLostFinger?.Invoke(lostFinger);
        }

        private void Touch_OnFingerMove(Finger movedFinger)
        {
            OnMovedFinger?.Invoke(movedFinger);
        }

        private void Touch_OnFingerDown(Finger touchedFinger)
        {
            if(IsClickingInsidePlayableArea(touchedFinger.screenPosition))
            {
                OnTouchedFinger?.Invoke(touchedFinger);
            }
        }

        private void Start()
        {
            playerInput.controlsChangedEvent.AddListener(OnControlsChanged);

            inputActions.Player.Quit.performed += Quit_performed;
        }

        private void Quit_performed(InputAction.CallbackContext callbackContext)
        {
            Application.Quit();
        }

        /// <summary>
        /// Only Valid for Editor Mode as Player can't click outside of the playable area while playing on an actual device
        /// </summary>
        /// <returns></returns>
        private bool IsClickingInsidePlayableArea(Vector2 screenPosition)
        {
            return (screenPosition.x >= 0f && screenPosition.x < screenWidth && screenPosition.y >= 0f && screenPosition.y < screenHeight);
        }

        private void OnControlsChanged(PlayerInput playerInput)
        {
            if (playerInput.currentControlScheme == KEYBOARD_MOUSE)
            {
                currentControlScheme = ControlScheme.KBM;
            }
            else if(playerInput.currentControlScheme == GAMEPAD)
            {
                currentControlScheme = ControlScheme.Gamepad;
            }
            else
            {
                currentControlScheme = ControlScheme.Touch;
            }
        }

        private void OnDisable()
        {
            inputActions.Disable();

            ETouch.Touch.onFingerDown -= Touch_OnFingerDown;
            ETouch.Touch.onFingerMove -= Touch_OnFingerMove;
            ETouch.Touch.onFingerUp -= Touch_OnFingerUp;

            EnhancedTouchSupport.Disable();
        }

        private void OnDestroy()
        {
            inputActions.Player.Quit.performed -= Quit_performed;
        }

        public static Vector2 GetNormalizedInputVector()
        {
            if (currentControlScheme != ControlScheme.Touch)
            {
                Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>().normalized;
                return inputVector;
            }
            else
            {
                return m_MovementFloatingJoystick.GetMovementAmount();
            }
        }

        public static Vector2 GetNormalizedLookVector()
        {
            if (currentControlScheme != ControlScheme.Touch)
            {
                return inputActions.Player.Look.ReadValue<Vector2>().normalized;
            }
            else
            {
                return m_CameraFloatingJoystick.GetMovementAmount();
            }
        }

        public static bool IsTouchInputOn()
        {
            return currentControlScheme == ControlScheme.Touch;
        }
    }
}