using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class CinematicCamera : MonoBehaviour
    {
        private const string CAMERA_INDEX = "Camera_Index";

        [SerializeField] private int cameraIndex;

        private Animator animator;
        private CinemachineCamera cinemachineCamera;

        private Vector3 originalPosition;
        private Vector3 originalRotation;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            cinemachineCamera = GetComponent<CinemachineCamera>();  

            originalPosition = transform.position;
            originalRotation = transform.localEulerAngles;

        }

        private void Start()
        {
            CameraSwitchManager.OnCameraChanged += CameraSwitchManager_OnCameraChanged;
        }

        private void CameraSwitchManager_OnCameraChanged(CinemachineCamera cinemachineCamera)
        {
            if (this.cinemachineCamera == cinemachineCamera)
            {
                animator.SetInteger(CAMERA_INDEX, cameraIndex);

                animator.enabled = true;
            }
            else
            {
                animator.SetInteger(CAMERA_INDEX, -1);

                animator.enabled = false;
                transform.position = originalPosition;
                transform.localEulerAngles = originalRotation;
            }
        }

        private void OnDestroy()
        {
            CameraSwitchManager.OnCameraChanged -= CameraSwitchManager_OnCameraChanged;
        }
    }
}