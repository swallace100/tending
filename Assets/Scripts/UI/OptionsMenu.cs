using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        AddApplyOnRelease(masterSlider);
        AddApplyOnRelease(bgmSlider);
        AddApplyOnRelease(fxSlider);
    }

    private void AddApplyOnRelease(Slider slider)
    {
        EventTrigger trigger = slider.GetComponent<EventTrigger>();
        if (!trigger) trigger = slider.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener(_ => Apply());
        trigger.triggers.Add(entry);
    }

    private void OnEnable()
    {
        RefreshOptions();
    }

    private void RefreshOptions()
    {
        masterSlider.value = Options.MasterVolume;
        bgmSlider.value = Options.BGMVolume;
        fxSlider.value = Options.SFXVolume;

        Apply();
    }

    public void Apply()
    {
        Options.MasterVolume = masterSlider.value;
        Options.BGMVolume = bgmSlider.value;
        Options.SFXVolume = fxSlider.value;
        Options.Save();

        audioMixer.SetFloat("MasterVolume", ToDecibels(Options.MasterVolume));
        audioMixer.SetFloat("BGMVolume", ToDecibels(Options.BGMVolume));
        audioMixer.SetFloat("SFXVolume", ToDecibels(Options.SFXVolume));
    }

    public void OnCloseButton()
    {
        Apply();
        gameObject.SetActive(false);
    }

    private float ToDecibels(float volume)
    {
        return volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
    }
}
