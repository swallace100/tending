using UnityEngine;

public class EventsHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private float defaultMusicVolume = 0.2f;
    [SerializeField] private float dreamMusicVolume = 0.1f;
    [SerializeField] private AudioClip dreamMusicIntro;
    [SerializeField] private AudioClip dreamMusicLoop;
    [SerializeField] private AudioClip feverMusicIntro;
    [SerializeField] private AudioClip feverMusicLoop;
    [SerializeField] private AudioClip lostMusicLoop;
    [SerializeField] private AudioClip outroMusicIntro;
    [SerializeField] private AudioClip outroMusicLoop;
    [SerializeField] private float outroMusicVolume = 0.15f;
    [SerializeField] private AudioClip dreamAmbiance;
    [SerializeField] private AudioClip homeAmbiance;

    public void PlayDreamMusic() { if (MusicManager.Instance) MusicManager.Instance.PlayMusicWithIntro(dreamMusicIntro, dreamMusicLoop, dreamMusicVolume); }
    public void PlayFeverMusic() { if (MusicManager.Instance) MusicManager.Instance.PlayMusicWithIntro(feverMusicIntro, feverMusicLoop, defaultMusicVolume); }
    public void PlayLostMusic() { if (MusicManager.Instance) MusicManager.Instance.PlayMusic(lostMusicLoop, dreamMusicVolume); }
    public void PlayOutroMusic() { if (MusicManager.Instance) MusicManager.Instance.PlayMusicWithIntro(outroMusicIntro, outroMusicLoop, outroMusicVolume); }
    public void PlayDreamAmbiance() { if (MusicManager.Instance) MusicManager.Instance.PlayAmbiance(dreamAmbiance); }
    public void PlayHomeAmbiance() { if (MusicManager.Instance) MusicManager.Instance.PlayAmbiance(homeAmbiance); }
    public void StopAmbiance() { if (MusicManager.Instance) MusicManager.Instance.StopAmbiance(); }
}
