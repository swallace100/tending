using TMPro;
using UnityEngine;
using UnityEngine.UI;

// The layout is landscape-only, and mobile browsers ignore the project's
// orientation lock, so on phones/tablets held upright we cover the screen
// with a gentle prompt to rotate. Created automatically; no scene setup needed.
public class RotateDevicePrompt : MonoBehaviour
{
    private const string Message = "Please turn your device sideways.\n\nTending works best in landscape.";

    private GameObject overlay;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Create()
    {
        if (!Application.isMobilePlatform) return;

        GameObject go = new GameObject("RotateDevicePrompt");
        DontDestroyOnLoad(go);
        go.AddComponent<RotateDevicePrompt>();
    }

    private void Awake()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;

        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0f;

        gameObject.AddComponent<GraphicRaycaster>();

        overlay = new GameObject("Overlay", typeof(RectTransform));
        overlay.transform.SetParent(transform, false);
        Stretch(overlay.GetComponent<RectTransform>(), 0f);

        Image background = overlay.AddComponent<Image>();
        background.color = new Color(0.12f, 0.11f, 0.10f, 1f);
        background.raycastTarget = true;

        GameObject textObject = new GameObject("Message", typeof(RectTransform));
        textObject.transform.SetParent(overlay.transform, false);
        Stretch(textObject.GetComponent<RectTransform>(), 100f);

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = Message;
        text.fontSize = 64;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(0.96f, 0.93f, 0.87f, 1f);
        text.raycastTarget = false;

        UpdateVisibility();
    }

    private void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        bool portrait = Screen.height > Screen.width;
        if (overlay.activeSelf != portrait) overlay.SetActive(portrait);
    }

    private static void Stretch(RectTransform rect, float margin)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(margin, margin);
        rect.offsetMax = new Vector2(-margin, -margin);
    }
}
