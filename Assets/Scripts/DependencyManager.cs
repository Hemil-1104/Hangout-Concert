using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DependencyManager : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;

        public static Transform PlayerTransform { get; private set; }

        public static PlayerDanceMovesManager PlayerDanceMovesManager { get; private set; }

        private void Awake()
        {
            PlayerTransform = playerTransform;
            PlayerDanceMovesManager = playerTransform.GetComponent<PlayerDanceMovesManager>();
        }
    }
}
