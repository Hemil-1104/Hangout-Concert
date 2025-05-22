using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class NPC : MonoBehaviour
    {
        public enum State
        {
            Vibing,
            Waving,
            Dancing
        }

        [SerializeField] private float stateChangeTimer = 10f;

        public event Action<State> OnStateChanged;

        private float currentTimer = 0f;
        private State currentState;

        private void Awake()
        {
            currentState = GetRandomState();
        }

        private void Update()
        {
            HandleStateChangeTimer();
        }

        private void HandleStateChangeTimer()
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > stateChangeTimer)
            {
                DoStateChange();
                currentTimer = 0f;
            }
        }

        private void DoStateChange()
        {
            currentState = GetRandomState();

            OnStateChanged?.Invoke(currentState);   
        }

        private State GetRandomState()
        {
            return (State)Random.Range(0, Enum.GetValues(typeof(State)).Length);
        }

        public State GetState()
        {
            return currentState;
        }
    }
}