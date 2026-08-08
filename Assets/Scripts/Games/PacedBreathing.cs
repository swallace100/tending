using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using TMPro;

public class PacedBreathing : MonoBehaviour
{
    [Header("Pacing")]
    [SerializeField] private float inhaleDuration = 7f;
    [SerializeField] private float exhaleDuration = 8f;

    [Header("Sound")]
    [SerializeField] private AudioSource breathAudioSource;
    [SerializeField] private AudioClip inhaleSound;
    [SerializeField] private AudioClip exhaleSound;
    [SerializeField] private AudioMixerGroup mixerGroup;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private string inhaleLabel = "Breathe In";
    [SerializeField] private string exhaleLabel = "Breathe Out";

    [Header("Optional Visual")]
    [SerializeField] private RectTransform breathingCircle;
    [SerializeField] private float inhaleScale = 1.3f;
    [SerializeField] private float exhaleScale = 0.9f;

    [SerializeField] private UnityEvent onStop;

    private Coroutine breathingRoutine;

    private void OnEnable()
    {
        if (breathAudioSource) breathAudioSource.outputAudioMixerGroup = mixerGroup;
        breathingRoutine = StartCoroutine(BreathingLoop());
    }

    private void OnDisable()
    {
        if (breathingRoutine != null) StopCoroutine(breathingRoutine);
        if (breathAudioSource) breathAudioSource.Stop();
    }

    // Wire a Stop button to this.
    public void StopBreathing()
    {
        if (breathingRoutine != null) StopCoroutine(breathingRoutine);
        breathingRoutine = null;
        if (breathAudioSource) breathAudioSource.Stop();
        onStop?.Invoke();
    }

    private IEnumerator BreathingLoop()
    {
        while (true)
        {
            yield return RunPhase(inhale: true, inhaleDuration, inhaleSound, inhaleLabel, exhaleScale, inhaleScale);
            yield return RunPhase(inhale: false, exhaleDuration, exhaleSound, exhaleLabel, inhaleScale, exhaleScale);
        }
    }

    private IEnumerator RunPhase(bool inhale, float duration, AudioClip sound, string label, float fromScale, float toScale)
    {
        if (phaseText) phaseText.text = label;

        if (breathAudioSource && sound)
        {
            breathAudioSource.clip = sound;
            breathAudioSource.loop = true;
            breathAudioSource.Play();
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (breathingCircle)
            {
                float scale = Mathf.Lerp(fromScale, toScale, elapsed / duration);
                breathingCircle.localScale = new Vector3(scale, scale, 1f);
            }
            yield return null;
        }

        if (breathAudioSource) breathAudioSource.Stop();
    }
}
