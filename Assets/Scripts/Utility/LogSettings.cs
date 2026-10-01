using UnityEngine;

public static class LogSettings
{
    // Silences Debug.Log output in release builds.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;
    }
}
