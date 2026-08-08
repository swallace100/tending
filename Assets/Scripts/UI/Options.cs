using UnityEngine;

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
}
