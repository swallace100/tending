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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        if (sfxSource == null) sfxSource = transform.Find("SFX")?.GetComponent<AudioSource>();
        if (ambianceSource == null) ambianceSource = transform.Find("Ambiance")?.GetComponent<AudioSource>();

        ApplyVolume();
        StartCoroutine(PlayBGMWhenReady());
    }

    private IEnumerator PlayBGMWhenReady()
    {
        while (!audioSource.isPlaying)
        {
            audioSource.Play();
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    public void PlaySFX(AudioClip clip, AudioMixerGroup mixerGroup, float pitch = 1f, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayAmbiance(AudioClip clip, float volume = 1f)
    {
        if (clip == null || ambianceSource == null) return;
        if (ambianceCoroutine != null) StopCoroutine(ambianceCoroutine);
        ambianceCoroutine = StartCoroutine(AmbianceCrossfadeRoutine(clip, volume));
    }

    public void StopAmbiance()
    {
        if (ambianceSource == null) return;
        if (ambianceCoroutine != null) StopCoroutine(ambianceCoroutine);
        ambianceCoroutine = StartCoroutine(AmbianceFadeOutRoutine());
    }

    private IEnumerator AmbianceCrossfadeRoutine(AudioClip newClip, float targetVolume)
    {
        float elapsed = 0f;
        float startVolume = ambianceSource.isPlaying ? ambianceSource.volume : 0f;

        while (elapsed < crossfadeDuration * 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            ambianceSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (crossfadeDuration * 0.5f));
            yield return null;
        }

        ambianceSource.Stop();
        ambianceSource.clip = newClip;
        ambianceSource.volume = 0f;
        ambianceSource.loop = true;
        ambianceSource.Play();

        elapsed = 0f;
        while (elapsed < crossfadeDuration * 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            ambianceSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / (crossfadeDuration * 0.5f));
            yield return null;
        }

        ambianceSource.volume = targetVolume;
    }

    private IEnumerator AmbianceFadeOutRoutine()
    {
        float elapsed = 0f;
        float startVolume = ambianceSource.volume;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            ambianceSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / crossfadeDuration);
            yield return null;
        }

        ambianceSource.Stop();
        ambianceSource.volume = startVolume;
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (musicCoroutine != null) StopCoroutine(musicCoroutine);
        musicCoroutine = StartCoroutine(CrossfadeRoutine(clip, volume));
    }

    public void PlayMusicWithIntro(AudioClip intro, AudioClip loop, float volume = 1f)
    {
        if (intro == null || loop == null) return;
        if (musicCoroutine != null) StopCoroutine(musicCoroutine);
        musicCoroutine = StartCoroutine(IntroLoopRoutine(intro, loop, volume));
    }

    public void StopMusic()
    {
        if (musicCoroutine != null) StopCoroutine(musicCoroutine);
        musicCoroutine = StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator CrossfadeRoutine(AudioClip newClip, float targetVolume)
    {
        float elapsed = 0f;
        float startVolume = audioSource.volume;

        while (elapsed < crossfadeDuration * 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (crossfadeDuration * 0.5f));
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.volume = 0f;
        audioSource.Play();

        elapsed = 0f;
        while (elapsed < crossfadeDuration * 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / (crossfadeDuration * 0.5f));
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private IEnumerator IntroLoopRoutine(AudioClip intro, AudioClip loop, float targetVolume)
    {
        float elapsed = 0f;
        float startVolume = audioSource.isPlaying ? audioSource.volume : targetVolume;

        while (elapsed < crossfadeDuration * 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (crossfadeDuration * 0.5f));
            yield return null;
        }

        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = intro;
        audioSource.volume = 0f;
        audioSource.Play();

        elapsed = 0f;
        while (elapsed < crossfadeDuration * 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / (crossfadeDuration * 0.5f));
            yield return null;
        }
        audioSource.volume = targetVolume;

        yield return new WaitUntil(() => !audioSource.isPlaying);

        audioSource.clip = loop;
        audioSource.loop = true;
        audioSource.Play();
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsed = 0f;
        float startVolume = audioSource.volume;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / crossfadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private void ApplyVolume()
    {
        audioMixer.SetFloat("MasterVolume", ToDecibels(Options.MasterVolume));
        audioMixer.SetFloat("BGMVolume", ToDecibels(Options.BGMVolume));
        audioMixer.SetFloat("SFXVolume", ToDecibels(Options.SFXVolume));
    }

    private float ToDecibels(float volume)
    {
        return volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
    }
}
