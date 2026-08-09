using UnityEngine;

public class EventsHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private float defaultMusicVolume = 1.0f;
    [SerializeField] private float defaultAmbientVolume = 1.0f;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip ambianceMusic;

    public void PlayMusic() { if (MusicManager.Instance) MusicManager.Instance.PlayMusic(gameMusic, defaultMusicVolume); }
    public void PlayAmbianceMusic() { if (MusicManager.Instance) MusicManager.Instance.PlayAmbiance(ambianceMusic, defaultAmbientVolume); }
    public void StopAmbiance() { if (MusicManager.Instance) MusicManager.Instance.StopAmbiance(); }
}
