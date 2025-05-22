using Unity.Cinemachine;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DancerScanCamera : MonoBehaviour
    {
        private CinemachineCamera cinemachineCamera;
        private CinemachineFollow cinemachineFollow;

        private void Awake()
        {
            cinemachineCamera = GetComponent<CinemachineCamera>();
            cinemachineFollow = GetComponent<CinemachineFollow>();
        }

        private void Start()
        {
            Dancer.OnScanDancer += Dancer_OnScanDancer;
        }

        private void Dancer_OnScanDancer(Transform dancerTransform)
        {
            cinemachineCamera.Follow = dancerTransform;
        }

        private void OnDestroy()
        {
            Dancer.OnScanDancer -= Dancer_OnScanDancer;
        }
    }
}
