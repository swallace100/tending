using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BodyRelaxation : MonoBehaviour
{
    [Serializable]
    public class BodyPartStep
    {
        public string bodyPartName;
        public bool canTense = true;
        [TextArea] public string tenseInstruction;
        [TextArea] public string relaxInstruction;
        public float tenseDuration = 10f;
        public float relaxDuration = 10f;
    }

    [SerializeField]
    private BodyPartStep[] steps = new BodyPartStep[]
    {
        new BodyPartStep { bodyPartName = "Forehead & Eyes", canTense = true,
            tenseInstruction = "Scrunch up your forehead and eyes tightly.",
            relaxInstruction = "Let your forehead and eyes go soft and loose." },
        new BodyPartStep { bodyPartName = "Jaw & Mouth", canTense = true,
            tenseInstruction = "Clench your jaw and press your lips together.",
            relaxInstruction = "Let your jaw hang loose and your lips part slightly." },
        new BodyPartStep { bodyPartName = "Neck & Throat", canTense = false,
            relaxInstruction = "Let your neck soften and your head feel heavy and supported." },
        new BodyPartStep { bodyPartName = "Shoulders", canTense = true,
            tenseInstruction = "Shrug your shoulders up toward your ears.",
            relaxInstruction = "Let your shoulders drop and sink down." },
        new BodyPartStep { bodyPartName = "Upper Arms", canTense = true,
            tenseInstruction = "Bend your elbows and squeeze your upper arms tight.",
            relaxInstruction = "Let your arms fall loose at your sides." },
        new BodyPartStep { bodyPartName = "Elbows", canTense = false,
            relaxInstruction = "Let your elbows soften and unlock, with no effort holding them in place." },
        new BodyPartStep { bodyPartName = "Forearms & Hands", canTense = true,
            tenseInstruction = "Clench your fists and tighten your forearms.",
            relaxInstruction = "Let your fingers uncurl and your hands go soft." },
        new BodyPartStep { bodyPartName = "Wrists", canTense = false,
            relaxInstruction = "Let your wrists go loose and heavy." },
        new BodyPartStep { bodyPartName = "Chest", canTense = true,
            tenseInstruction = "Take a deep breath in and hold it, tightening your chest.",
            relaxInstruction = "Breathe out slowly and let your chest settle." },
        new BodyPartStep { bodyPartName = "Upper Back", canTense = true,
            tenseInstruction = "Squeeze your shoulder blades together.",
            relaxInstruction = "Let your back settle and widen." },
        new BodyPartStep { bodyPartName = "Stomach", canTense = true,
            tenseInstruction = "Tighten your stomach muscles.",
            relaxInstruction = "Let your stomach soften with each breath." },
        new BodyPartStep { bodyPartName = "Lower Back", canTense = false,
            relaxInstruction = "Let your lower back settle into a comfortable, supported position." },
        new BodyPartStep { bodyPartName = "Hips", canTense = false,
            relaxInstruction = "Let your hips feel heavy and supported, with nothing to hold up." },
        new BodyPartStep { bodyPartName = "Glutes", canTense = true,
            tenseInstruction = "Squeeze your glutes tight.",
            relaxInstruction = "Let them soften completely." },
        new BodyPartStep { bodyPartName = "Thighs", canTense = true,
            tenseInstruction = "Tighten the muscles in your thighs.",
            relaxInstruction = "Let your thighs go heavy and loose." },
        new BodyPartStep { bodyPartName = "Knees", canTense = false,
            relaxInstruction = "Let your knees soften and unlock." },
        new BodyPartStep { bodyPartName = "Calves", canTense = true,
            tenseInstruction = "Point your toes and tighten your calves.",
            relaxInstruction = "Let your calves relax completely." },
        new BodyPartStep { bodyPartName = "Ankles", canTense = false,
            relaxInstruction = "Let your ankles go loose and heavy." },
        new BodyPartStep { bodyPartName = "Feet & Toes", canTense = true,
            tenseInstruction = "Curl your toes tightly.",
            relaxInstruction = "Let your toes and feet go fully loose." },
    };

    [Header("Panels")]
    [SerializeField] private GameObject stepPanel;
    [SerializeField] private GameObject completePanel;

    [Header("Step Panel References")]
    [SerializeField] private TextMeshProUGUI bodyPartText;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerFillImage;

    [Header("Complete Panel References")]
    [SerializeField] private TextMeshProUGUI completeText;
    [TextArea][SerializeField] private string completeMessage = "You've relaxed your whole body. Carry this feeling with you today.";

    private bool isPaused;
    private bool skipRequested;
    private Coroutine sequenceRoutine;

    private void OnEnable()
    {
        isPaused = false;
        skipRequested = false;
        completePanel.SetActive(false);
        stepPanel.SetActive(true);
        sequenceRoutine = StartCoroutine(RunSequence());
    }

    private void OnDisable()
    {
        if (sequenceRoutine != null) StopCoroutine(sequenceRoutine);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    public void SkipStep()
    {
        skipRequested = true;
    }

    private IEnumerator RunSequence()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            BodyPartStep step = steps[i];
            progressText.text = $"{i + 1} / {steps.Length}";
            bodyPartText.text = step.bodyPartName;

            if (step.canTense)
                yield return RunPhase(step.tenseInstruction, step.tenseDuration);

            yield return RunPhase(step.relaxInstruction, step.relaxDuration);
        }

        stepPanel.SetActive(false);
        completePanel.SetActive(true);
        if (completeText) completeText.text = completeMessage;
    }

    private IEnumerator RunPhase(string instruction, float duration)
    {
        instructionText.text = instruction;
        skipRequested = false;

        float remaining = duration;
        while (remaining > 0f && !skipRequested)
        {
            if (!isPaused)
            {
                remaining -= Time.deltaTime;
                UpdateTimerDisplay(Mathf.Max(remaining, 0f), duration);
            }
            yield return null;
        }

        UpdateTimerDisplay(0f, duration);
    }

    private void UpdateTimerDisplay(float remaining, float duration)
    {
        if (timerText) timerText.text = Mathf.CeilToInt(remaining).ToString();
        if (timerFillImage) timerFillImage.fillAmount = duration > 0f ? remaining / duration : 0f;
    }
}
