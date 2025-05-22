using UnityEngine;

namespace nostra.booboogames.hangoutconcert
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ThirdPersonCameraRotationManager cameraManager;

        private Camera mainCamera;

        private bool isMovementBlocked = false;
        private float movementSpeed;
        private Vector2 movementVector;
        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            movementSpeed = Player.MovementSpeed;

            PlayerAnimator.OnStartedDancing += PlayerAnimator_OnStartedDancing;

            ButtonsHandler.OnStopDancing += ButtonsHandler_OnStopDancing;

            DJBooth.OnStartedPlayingDJBoothGame += DJBooth_OnStartedPlayingDJBoothGame;

            DJBoothMinigameUI.OnGameClosed += DJBoothMinigameUI_OnGameClosed;
        }

        private void DJBoothMinigameUI_OnGameClosed()
        {
            isMovementBlocked = false;
        }

        private void DJBooth_OnStartedPlayingDJBoothGame()
        {
            isMovementBlocked = true;
        }

        private void ButtonsHandler_OnStopDancing()
        {
            isMovementBlocked = false;
        }

        private void PlayerAnimator_OnStartedDancing()
        {
            isMovementBlocked = true;
        }

        private void FixedUpdate()
        {
            if (isMovementBlocked) return;

            HandleMovement();
        }

        private void HandleMovement()
        {
            movementVector = InputManager.GetNormalizedInputVector();
            Vector3 movementDirection = GetCameraRelativeMovementDirection(movementVector);

            Vector3 actualMovementVector = movementSpeed * Time.fixedDeltaTime * movementDirection;

            characterController.Move(actualMovementVector);

            if (movementVector.sqrMagnitude > 0f)
            {
                transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg, 0f);
            }
        }

        private Vector3 GetCameraRelativeMovementDirection(Vector2 movementVector)
        {
            float horizontalInput = movementVector.x;
            float verticalInput = movementVector.y;

            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // Transform the movement Vector in the camera's local coordinate space by multiplying the respective axis with its forward and right vectors
            Vector3 forwardRelativeHorizontalInput = horizontalInput * right;
            Vector3 forwardRelativeVerticalInput = verticalInput * forward;
            Vector3 movementDirection = forwardRelativeHorizontalInput + forwardRelativeVerticalInput;

            return movementDirection;
        }

        private void OnDestroy()
        {
            PlayerAnimator.OnStartedDancing -= PlayerAnimator_OnStartedDancing;

            ButtonsHandler.OnStopDancing -= ButtonsHandler_OnStopDancing;

            DJBooth.OnStartedPlayingDJBoothGame += DJBooth_OnStartedPlayingDJBoothGame;

            DJBoothMinigameUI.OnGameClosed -= DJBoothMinigameUI_OnGameClosed;
        }

        public bool IsMoving()
        {
            return !movementVector.Equals(Vector2.zero);
        }
    }
}