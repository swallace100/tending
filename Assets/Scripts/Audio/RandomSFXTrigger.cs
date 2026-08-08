using UnityEngine;
using UnityEngine.Audio;

public class RandomSFXTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField][Range(0f, 1f)] private float volume = 1f;
    [SerializeField] private bool triggerOnce = true;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (triggerOnce && triggered) return;
        if (clips == null || clips.Length == 0) return;

        triggered = true;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        MusicManager.Instance?.PlaySFX(clip, mixerGroup, 1f, volume);
    }
}
