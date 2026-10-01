using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

[CreateAssetMenu(fileName = "UIButtonSoundSettings", menuName = "UI Button Sound Settings")]
public class UIButtonSoundSettings : ScriptableObject
{
    public AudioClip hoverSound;
    public AudioClip clickSound;
    [Range(0f, 1f)] public float volume = 1f;

    [Header("Cursor")]
    public Texture2D handCursor;
    public Vector2 handCursorHotspot;
}

public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private UIButtonSoundSettings settings;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // A touch "enters" on the same tap that clicks, so skip hover feedback
        // to avoid playing both sounds at once.
        if (IsTouch(eventData)) return;

        if (settings && settings.hoverSound)
            MusicManager.Instance?.PlaySFX(settings.hoverSound, settings.volume);

        if (settings && settings.handCursor)
            Cursor.SetCursor(settings.handCursor, settings.handCursorHotspot, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (settings && settings.handCursor)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (settings && settings.clickSound)
            MusicManager.Instance?.PlaySFX(settings.clickSound, settings.volume);

        if (settings && settings.handCursor)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private static bool IsTouch(PointerEventData eventData)
    {
        return eventData is ExtendedPointerEventData extended && extended.pointerType == UIPointerType.Touch;
    }
}
