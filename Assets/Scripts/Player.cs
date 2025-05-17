using System;
using UnityEngine;

namespace nostra.booboogames.hangoutconcert
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;

        public static float MovementSpeed { get; private set; }
        public static float Energy { get; private set; }
        public static float EnergyDepletionRate { get; private set; }

        private static AnimationCurve movementSpeedMultiplierCurve;

        private void Awake()
        {
            MovementSpeed = playerStats.MovementSpeed;
            Energy = playerStats.Energy;
            EnergyDepletionRate = playerStats.EnergyDepletionRate;

            movementSpeedMultiplierCurve = playerStats.MovementSpeedMultiplier;
        }

        public static float GetMovementSpeedMultiplierAtRemainingEnergy(float energyPercentage)
        {
            return movementSpeedMultiplierCurve.Evaluate(energyPercentage);
        }
    }
}