using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Put one of these on each scene's root Canvas. It records every TMP text's
// authored font size once, then multiplies by AccessibilityOptions.TextScale,
// including texts on currently inactive panels.
public class TextScaler : MonoBehaviour
{
    private readonly Dictionary<TMP_Text, float> baseSizes = new();
    private readonly Dictionary<TMP_Text, (float min, float max)> baseAutoSizes = new();

    private void Awake()
    {
        foreach (TMP_Text text in GetComponentsInChildren<TMP_Text>(includeInactive: true))
        {
            baseSizes[text] = text.fontSize;
            baseAutoSizes[text] = (text.fontSizeMin, text.fontSizeMax);
        }
    }

    private void OnEnable()
    {
        AccessibilityOptions.Changed += ApplyScale;
        ApplyScale();
    }

    private void OnDisable()
    {
        AccessibilityOptions.Changed -= ApplyScale;
    }

    private void ApplyScale()
    {
        float scale = AccessibilityOptions.TextScale;
        foreach (KeyValuePair<TMP_Text, float> entry in baseSizes)
        {
            TMP_Text text = entry.Key;
            if (!text) continue;

            if (text.enableAutoSizing)
            {
                (float min, float max) = baseAutoSizes[text];
                text.fontSizeMin = min * scale;
                text.fontSizeMax = max * scale;
            }
            else
            {
                text.fontSize = entry.Value * scale;
            }
        }
    }
}
