using UnityEngine;
using UnityEngine.Audio;

public class RandomSFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField][Range(0f, 1f)] private float volume = 1f;
    [SerializeField][Range(0.5f, 2f)] private float pitch = 1f;
    [SerializeField][Range(0f, 1f)] private float playChance = 0.75f;

    public void Play()
    {
        if (clips == null || clips.Length == 0) return;
        if (Random.value > playChance) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        MusicManager.Instance?.PlaySFX(clip, mixerGroup, pitch, volume);
    }
}
