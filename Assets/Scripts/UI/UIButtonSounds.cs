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
}

public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private UIButtonSoundSettings settings;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (settings && settings.hoverSound)
            MusicManager.Instance?.PlaySFX(settings.hoverSound, settings.mixerGroup, 1f, settings.volume);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (settings && settings.clickSound)
            MusicManager.Instance?.PlaySFX(settings.clickSound, settings.mixerGroup, 1f, settings.volume);
    }
}
