using UnityEngine;

namespace nostra.booboogames.hangoutconcert
{
    [CreateAssetMenu(fileName = "Player Stats", menuName = "Player Stats")]
    public class PlayerStats : ScriptableObject
    {
        public float MovementSpeed;
        public float Energy;
        [Tooltip("Per Second")]
        public float EnergyDepletionRate;
        public AnimationCurve MovementSpeedMultiplier;
    }
}