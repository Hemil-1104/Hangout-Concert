using UnityEngine;
using UnityEngine.InputSystem;

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

        private static PlayerInputActions inputActions;
        private static FloatingJoystick m_CameraFloatingJoystick;
        private static FloatingJoystick m_MovementFloatingJoystick;
        private static ControlScheme currentControlScheme;

        private PlayerInput playerInput;

        private void Awake()
        {
            inputActions = new PlayerInputActions();

            m_CameraFloatingJoystick = cameraFloatingJoystick;
            m_MovementFloatingJoystick = movementFloatingJoystick;

            playerInput = GetComponent<PlayerInput>();
            currentControlScheme = ControlScheme.Touch;
        }

        private void Start()
        {
            playerInput.controlsChangedEvent.AddListener(OnControlsChanged);
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

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
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