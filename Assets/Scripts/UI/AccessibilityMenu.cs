using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Add to the Options panel alongside OptionsMenu. Wire a text-size slider
// (whole numbers work well: min 8, max 16, value = scale x 10) and a
// reduce-motion toggle. Both references are optional so this can be wired
// incrementally.
public class AccessibilityMenu : MonoBehaviour
{
    [SerializeField] private Slider textScaleSlider;
    [SerializeField] private Toggle reduceMotionToggle;
    [SerializeField] private TextMeshProUGUI textScalePreview;

    private void OnEnable()
    {
        if (textScaleSlider)
        {
            textScaleSlider.SetValueWithoutNotify(AccessibilityOptions.TextScale * 10f);
            textScaleSlider.onValueChanged.AddListener(OnTextScaleChanged);
        }
        if (reduceMotionToggle)
        {
            reduceMotionToggle.SetIsOnWithoutNotify(AccessibilityOptions.ReduceMotion);
            reduceMotionToggle.onValueChanged.AddListener(OnReduceMotionChanged);
        }
        UpdatePreview();
    }

    private void OnDisable()
    {
        if (textScaleSlider) textScaleSlider.onValueChanged.RemoveListener(OnTextScaleChanged);
        if (reduceMotionToggle) reduceMotionToggle.onValueChanged.RemoveListener(OnReduceMotionChanged);
    }

    private void OnTextScaleChanged(float sliderValue)
    {
        AccessibilityOptions.TextScale = sliderValue / 10f;
        AccessibilityOptions.Save();
        UpdatePreview();
    }

    private void OnReduceMotionChanged(bool isOn)
    {
        AccessibilityOptions.ReduceMotion = isOn;
        AccessibilityOptions.Save();
    }

    private void UpdatePreview()
    {
        if (textScalePreview)
            textScalePreview.text = $"{Mathf.RoundToInt(AccessibilityOptions.TextScale * 100f)}%";
    }
}
