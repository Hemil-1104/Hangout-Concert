using UnityEngine;

namespace MyGames
{
    public static class Debug
    {
        public static void Log<T>(T message, MonoBehaviour context = null)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log($"{message}\n{StackTraceUtility.ExtractStackTrace()}", context);
#endif
        }

        public static void LogError(string message, MonoBehaviour context = null)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message, context);
#endif
        }

        public static void LogWarning(string message, MonoBehaviour context = null)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message, context);
#endif
        }
    }
}
