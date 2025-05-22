using System;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class LevelPromptTrigger : MonoBehaviour
    {
        public static event Action OnPlayerEnteredLevelTrigger;
        public static event Action OnPlayerExitedLevelTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Player player))
            {
                OnPlayerEnteredLevelTrigger?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                OnPlayerExitedLevelTrigger?.Invoke();
            }
        }
    }
}
