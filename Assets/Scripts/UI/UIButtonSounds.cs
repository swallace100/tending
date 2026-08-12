using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "UIButtonSoundSettings", menuName = "UI Button Sound Settings")]
public class UIButtonSoundSettings : ScriptableObject
{
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioMixerGroup mixerGroup;
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
        if (settings && settings.hoverSound)
            MusicManager.Instance?.PlaySFX(settings.hoverSound, settings.mixerGroup, 1f, settings.volume);

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
            MusicManager.Instance?.PlaySFX(settings.clickSound, settings.mixerGroup, 1f, settings.volume);
    }
}
