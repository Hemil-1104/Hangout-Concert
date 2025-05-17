using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyGames
{
    public static class Utils
    {
        public static int ConvertLayerMaskValueToLayerNumber(int layerMaskValue)
        {
            for(int i = 0; i < 32; i++)
            {
                if((layerMaskValue & (1 << i)) >= 1)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int ConvertLayerNumberToLayerMaskValue(int layerNumber)
        {
            return (int)System.MathF.Pow(2, layerNumber);
        }

        /// <summary>
        /// Takes a list and returns a random index
        /// </summary>
        /// <param name="list"></param>
        /// <param name="startIndex">The start index of the list. -1 if want to start from the 0</param>
        /// <param name="endIndex">The last index of the list. -1 if want to end at the last index</param>
        /// <returns></returns>
        public static int GetRandomIndexFromList<T>(List<T> list, int startIndex = -1, int endIndex = -1)
        {
            int start = startIndex == -1 ? 0 : startIndex;
            int end = endIndex == -1 ? list.Count : endIndex;

            return Random.Range(start, end);
        }

        public static float GetRandomValueBetween(float min, float max)
        {
            return Random.Range(min, max);
        }

        public static string GetDJBoothMinigameResultText(int totalPoints, int totalPointsPossible)
        {
            float percentage = totalPoints / (float)totalPointsPossible;
            return $"You Scored {totalPoints} Points with a Rating of {percentage * 100f}%!";
        }

        public static float GetDistance(Vector3 position1, Vector3 position2, bool excludeX = false, bool excludeY = false, bool excludeZ = false)
        {
            Vector3 normalizedPosition1 = new Vector3(excludeX ? 0f : position1.x, excludeY ? 0f : position1.y, excludeZ ? 0f : position1.z);
            Vector3 normalizedPosition2 = new Vector3(excludeX ? 0f : position2.x, excludeY ? 0f : position2.y, excludeZ ? 0f : position2.z);

            return Vector3.Distance(normalizedPosition1, normalizedPosition2);
        }

        public static Vector3 GetRandomPositionInObjectGeometry(GameObject gameObject, bool excludeX = false, bool exculdeY = false, bool excludeZ = false)
        {
            Vector3 position = gameObject.transform.position;
            Vector3 scale = gameObject.transform.localScale;

            float minX = position.x - (scale.x / 2f);
            float maxX = position.x + (scale.x / 2f);
            float minY = position.y - (scale.y / 2f);
            float maxY = position.y + (scale.y / 2f);
            float minZ = position.z - (scale.z / 2f);
            float maxZ = position.z + (scale.z / 2f);

            return new Vector3(excludeX ? 0f : Random.Range(minX, maxX), exculdeY ? 0f : Random.Range(minY, maxY), excludeZ ? 0f : Random.Range(minZ, maxZ));
        }
    }
}