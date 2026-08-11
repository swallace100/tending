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
        [TextArea] public string instruction;
    }

    [SerializeField]
    private float instructionDuration = 12f;

    [SerializeField]
    private BodyPartStep[] steps = new BodyPartStep[]
    {
        new BodyPartStep { bodyPartName = "Forehead & Eyes",
            instruction = "Scrunch up your forehead and eyes tightly, then relax." },
        new BodyPartStep { bodyPartName = "Jaw & Mouth",
            instruction = "Clench your jaw and press your lips together, then relax." },
        new BodyPartStep { bodyPartName = "Neck & Throat",
            instruction = "Let your neck and throat let go of any stress." },
        new BodyPartStep { bodyPartName = "Shoulders",
            instruction = "Shrug your shoulders up toward your ears, then relax." },
        new BodyPartStep { bodyPartName = "Upper Arms",
            instruction = "Bend your elbows and squeeze your upper arms tight, then relax." },
        new BodyPartStep { bodyPartName = "Elbows",
            instruction = "Let your elbows let go of any stress." },
        new BodyPartStep { bodyPartName = "Forearms & Hands",
            instruction = "Clench your fists and tighten your forearms, then relax." },
        new BodyPartStep { bodyPartName = "Wrists",
            instruction = "Let your wrists let go of any stress." },
        new BodyPartStep { bodyPartName = "Fingers",
            instruction = "Stretch your fingers out, then relax." },
        new BodyPartStep { bodyPartName = "Chest",
            instruction = "Tighten your chest muscles and breath in, then breath out and relax." },
        new BodyPartStep { bodyPartName = "Upper Back",
            instruction = "Squeeze your shoulder blades together, then relax." },
        new BodyPartStep { bodyPartName = "Stomach",
            instruction = "Tighten your stomach muscles, then relax." },
        new BodyPartStep { bodyPartName = "Lower Back",
            instruction = "Let your lower back let go of any stress and relax."},
        new BodyPartStep { bodyPartName = "Hips",
            instruction = "Let your hips feel let go of any stress and relax." },
        new BodyPartStep { bodyPartName = "Glutes",
            instruction = "Squeeze your glutes tight, then relax."},
        new BodyPartStep { bodyPartName = "Thighs",
            instruction = "Tighten the muscles in your thighs, then relax." },
        new BodyPartStep { bodyPartName = "Knees",
            instruction = "Let your knees let go of any stress and relax." },
        new BodyPartStep { bodyPartName = "Calves",
            instruction = "Point your toes and tighten your calves, then relax." },
        new BodyPartStep { bodyPartName = "Ankles",
            instruction = "Let your ankles let go of any stress and relax." },
        new BodyPartStep { bodyPartName = "Feet & Toes",
            instruction = "Curl your toes tightly, then relax." },
    };

    [Header("Panels")]
    [SerializeField] private GameObject stepPanel;
    [SerializeField] private GameObject completePanel;

    [Header("Step Panel References")]
    [SerializeField] private TextMeshProUGUI bodyPartText;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private TextMeshProUGUI progressText;
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

            yield return RunPhase(step.instruction);
        }

        stepPanel.SetActive(false);
        completePanel.SetActive(true);
        if (completeText) completeText.text = completeMessage;
    }

    private IEnumerator RunPhase(string instruction)
    {
        instructionText.text = instruction;
        skipRequested = false;

        float remaining = instructionDuration;
        while (remaining > 0f && !skipRequested)
        {
            if (!isPaused)
            {
                remaining -= Time.deltaTime;
                UpdateTimerDisplay(Mathf.Max(remaining, 0f), instructionDuration);
            }
            yield return null;
        }

        UpdateTimerDisplay(0f, instructionDuration);
    }

    private void UpdateTimerDisplay(float remaining, float duration)
    {
        if (timerFillImage) timerFillImage.fillAmount = duration > 0f ? remaining / duration : 0f;
    }
}
