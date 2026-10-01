using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambianceSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float crossfadeDuration = 1f;

    private Coroutine musicCoroutine;
    private Coroutine ambianceCoroutine;

    // The clip each channel is playing or fading towards (null once stopped).
    // Used to skip restarting a track that's already playing, e.g. when
    // returning to the main menu.
    private AudioClip musicTarget;
    private AudioClip ambianceTarget;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (sfxSource == null) sfxSource = transform.Find("SFX")?.GetComponent<AudioSource>();
        if (ambianceSource == null) ambianceSource = transform.Find("Ambiance")?.GetComponent<AudioSource>();

        musicTarget = audioSource.playOnAwake ? audioSource.clip : null;
        ambianceTarget = ambianceSource != null && ambianceSource.playOnAwake ? ambianceSource.clip : null;

        ApplyVolume();
        StartCoroutine(ReapplyVolumeNextFrame());
        StartCoroutine(PlayBGMWhenReady());
    }

    private IEnumerator ReapplyVolumeNextFrame()
    {
        // In the Editor, AudioMixer.SetFloat calls made in Awake can be silently
        // dropped because the mixer's DSP graph isn't built yet. Reapplying a
        // frame later ensures the saved volume actually takes effect.
        yield return null;
        ApplyVolume();
    }

    private IEnumerator PlayBGMWhenReady()
    {
        // Keeps retrying until playback actually starts (e.g. WebGL blocks audio
        // until the first user interaction). Gives up if the music is stopped.
        while (musicTarget != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    public void ApplyVolume()
    {
        Options.ApplyTo(audioMixer);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null || clip == musicTarget) return;
        musicTarget = clip;
        if (musicCoroutine != null) StopCoroutine(musicCoroutine);
        musicCoroutine = StartCoroutine(CrossfadeRoutine(audioSource, clip, volume, true));
    }

    public void PlayMusicWithIntro(AudioClip intro, AudioClip loop, float volume = 1f)
    {
        if (intro == null || loop == null || loop == musicTarget) return;
        musicTarget = loop;
        if (musicCoroutine != null) StopCoroutine(musicCoroutine);
        musicCoroutine = StartCoroutine(IntroLoopRoutine(intro, loop, volume));
    }

    public void StopMusic()
    {
        musicTarget = null;
        if (musicCoroutine != null) StopCoroutine(musicCoroutine);
        musicCoroutine = StartCoroutine(FadeOutRoutine(audioSource));
    }

    public void PlayAmbiance(AudioClip clip, float volume = 1f)
    {
        if (clip == null || ambianceSource == null || clip == ambianceTarget) return;
        ambianceTarget = clip;
        if (ambianceCoroutine != null) StopCoroutine(ambianceCoroutine);
        ambianceCoroutine = StartCoroutine(CrossfadeRoutine(ambianceSource, clip, volume, true));
    }

    public void StopAmbiance()
    {
        if (ambianceSource == null) return;
        ambianceTarget = null;
        if (ambianceCoroutine != null) StopCoroutine(ambianceCoroutine);
        ambianceCoroutine = StartCoroutine(FadeOutRoutine(ambianceSource));
    }

    private IEnumerator CrossfadeRoutine(AudioSource source, AudioClip newClip, float targetVolume, bool loop)
    {
        float halfDuration = crossfadeDuration * 0.5f;

        if (source.isPlaying)
            yield return FadeRoutine(source, source.volume, 0f, halfDuration);

        source.Stop();
        source.clip = newClip;
        source.loop = loop;
        source.volume = 0f;
        source.Play();

        yield return FadeRoutine(source, 0f, targetVolume, halfDuration);
    }

    private IEnumerator IntroLoopRoutine(AudioClip intro, AudioClip loop, float targetVolume)
    {
        yield return CrossfadeRoutine(audioSource, intro, targetVolume, false);
        yield return new WaitUntil(() => !audioSource.isPlaying);

        audioSource.clip = loop;
        audioSource.loop = true;
        audioSource.Play();
    }

    private IEnumerator FadeOutRoutine(AudioSource source)
    {
        float startVolume = source.volume;
        yield return FadeRoutine(source, startVolume, 0f, crossfadeDuration);

        source.Stop();
        source.volume = startVolume;
    }

    private IEnumerator FadeRoutine(AudioSource source, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        source.volume = to;
    }
}
