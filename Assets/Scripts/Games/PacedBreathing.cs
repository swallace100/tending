using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
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
    private bool isPaused;
    private Image breathingCircleImage;

    private void OnEnable()
    {
        isPaused = false;
        if (breathAudioSource) breathAudioSource.outputAudioMixerGroup = mixerGroup;
        if (breathingCircle) breathingCircleImage = breathingCircle.GetComponent<Image>();
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

    // Wire a Pause button to this.
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (breathAudioSource)
        {
            if (isPaused) breathAudioSource.Pause();
            else breathAudioSource.UnPause();
        }
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
            if (!isPaused)
            {
                elapsed += Time.deltaTime;
                if (breathingCircle)
                {
                    // Reduce-motion: keep the circle still; the label and fill timer still pace the breath.
                    float scale = AccessibilityOptions.ReduceMotion
                        ? 1f
                        : Mathf.Lerp(fromScale, toScale, elapsed / duration);
                    breathingCircle.localScale = new Vector3(scale, scale, 1f);
                }
                if (breathingCircleImage)
                {
                    breathingCircleImage.fillAmount = 1f - (elapsed / duration);
                }
            }
            yield return null;
        }

        if (breathAudioSource) breathAudioSource.Stop();
    }
}
