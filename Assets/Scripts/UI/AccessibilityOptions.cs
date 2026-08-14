using System;
using UnityEngine;

// Device-local UI preferences only (PlayerPrefs). No personal data, usage
// stats, or exercise content is ever stored - keep it that way.
public class AccessibilityOptions
{
    public const float MinTextScale = 0.8f;
    public const float MaxTextScale = 1.6f;

    public static float TextScale = PlayerPrefs.GetFloat("TextScale", 1f);
    public static bool ReduceMotion = PlayerPrefs.GetInt("ReduceMotion", 0) == 1;

    // Fired after Save() so open scenes can re-apply (e.g. TextScaler).
    public static event Action Changed;

    public static void Save()
    {
        TextScale = Mathf.Clamp(TextScale, MinTextScale, MaxTextScale);
        PlayerPrefs.SetFloat("TextScale", TextScale);
        PlayerPrefs.SetInt("ReduceMotion", ReduceMotion ? 1 : 0);
        PlayerPrefs.Save();
        Changed?.Invoke();
    }
}
