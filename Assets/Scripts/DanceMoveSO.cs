using UnityEditor;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    [CreateAssetMenu()]
    public class DanceMoveSO : ScriptableObject
    {
        public enum MoveType
        {
            Starter = 0,
            Scanned = 1,
            Purchased = 2
        }

        public int DanceMoveIndex;
        public Sprite DanceMoveSprite;
        public int DanceMoveXpReward;
        public MoveType DanceMoveType;
        public float DanceMoveMasteryWeight;

        [HideInInspector] public int DanceMoveCost;

#if UNITY_EDITOR

        [CanEditMultipleObjects]
        [CustomEditor(typeof(DanceMoveSO))]
        public class DanceMoveSOCustomEditor : Editor
        {
            public override void OnInspectorGUI ()
            {
                base.OnInspectorGUI();

                DanceMoveSO danceMoveSO = target as DanceMoveSO;

                if (danceMoveSO.DanceMoveType == MoveType.Purchased)
                {
                    danceMoveSO.DanceMoveCost = EditorGUILayout.IntField("Dance Move Cost", danceMoveSO.DanceMoveCost);
                }

                EditorUtility.SetDirty(danceMoveSO);
            }
        }

#endif
    }
}
