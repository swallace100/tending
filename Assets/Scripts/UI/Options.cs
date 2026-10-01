using UnityEngine;
using UnityEngine.Audio;

public class Options
{
    public static float MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    public static float BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
    public static float SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

    public static void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.Save();
    }

    public static void ApplyTo(AudioMixer mixer)
    {
        if (mixer == null) return;
        mixer.SetFloat("MasterVolume", ToDecibels(MasterVolume));
        mixer.SetFloat("BGMVolume", ToDecibels(BGMVolume));
        mixer.SetFloat("SFXVolume", ToDecibels(SFXVolume));
    }

    private static float ToDecibels(float volume)
    {
        return volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
    }
}
