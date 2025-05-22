using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class CameraSwitchManager : MonoBehaviour
    {
        private const int MIN_PRIORITY = 10;
        private const int MAX_PRIORITY = 20;

        [SerializeField] private CinematicCameraNavigationUI cinematicCameraNavigationUI;
        [SerializeField] private CinemachineCamera playerFollowCamera;
        [SerializeField] private CinemachineCamera dancerScanCamera;

        [SerializeField] private List<CinemachineCamera> cinematicCamerasList;

        public static event Action<CinemachineCamera> OnCameraChanged;

        private int currentCinematicCameraIndex = 0;
        private int totalCameras;
        private CinemachineCamera currentActiveCamera;

        private void Awake()
        {
            totalCameras = cinematicCamerasList.Count;
            currentActiveCamera = playerFollowCamera;
        }

        private void Start()
        {
            CinematicCameraNavigationUI.OnOpenCinematicCamera += CinematicCameraNavigationUI_OnOpenCinematicCamera;
            CinematicCameraNavigationUI.OnCloseCinematicCamera += CinematicCameraNavigationUI_OnCloseCinematicCamera;

            Dancer.OnScanDancer += Dancer_OnScanDancer;

            DanceMoveScanUI.OnGetOutOfScanMode += DanceMoveScanUI_OnGetOutOfScanMode;
        }

        private void DanceMoveScanUI_OnGetOutOfScanMode()
        {
            SwitchToCamera(playerFollowCamera);
        }

        private void Dancer_OnScanDancer(Transform transform)
        {
            SwitchToCamera(dancerScanCamera);
        }

        private void CinematicCameraNavigationUI_OnOpenCinematicCamera()
        {
            cinematicCameraNavigationUI.ToggleUIContainerVisibility(true);

            SwitchToNewCinematicCamera(0);
        }

        private void CinematicCameraNavigationUI_OnCloseCinematicCamera()
        {
            SwitchToCamera(playerFollowCamera);
        }

        private void SwitchToCamera(CinemachineCamera cinemachineCamera)
        {
            currentActiveCamera.Priority = MIN_PRIORITY;
            cinemachineCamera.Priority = MAX_PRIORITY;

            currentActiveCamera = cinemachineCamera;

            OnCameraChanged?.Invoke(currentActiveCamera);
        }

        private void SwitchToNewCinematicCamera(int add)
        {
            currentCinematicCameraIndex = (currentCinematicCameraIndex + add + totalCameras) % totalCameras;

            SwitchToCamera(cinematicCamerasList[currentCinematicCameraIndex]);
        }

        public void SwitchToNewCinematicCamera(bool isNext)
        {
            SwitchToNewCinematicCamera(isNext ? 1 : -1);   
        }

        private void OnDestroy()
        {
            CinematicCameraNavigationUI.OnOpenCinematicCamera -= CinematicCameraNavigationUI_OnOpenCinematicCamera;
            CinematicCameraNavigationUI.OnCloseCinematicCamera -= CinematicCameraNavigationUI_OnCloseCinematicCamera;

            Dancer.OnScanDancer -= Dancer_OnScanDancer;

            DanceMoveScanUI.OnGetOutOfScanMode -= DanceMoveScanUI_OnGetOutOfScanMode;
        }
    }
}
