using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class GroundingExercise : MonoBehaviour
{
    [Serializable]
    public class SensePhase
    {
        public string senseLabel;
        public int itemCount;
        [TextArea] public string instruction;
        public string buttonLabel;
        public string summaryLead;
    }

    [Header("Orientation Questions")]
    [SerializeField]
    private string[] orientationQuestions = new string[]
    {
        "What year is it?",
        "What season is it?",
        "What month is it?",
    };

    [Header("Sense Phases (5-4-3-2-1)")]
    [SerializeField]
    private SensePhase[] sensePhases = new SensePhase[]
    {
        new SensePhase
        {
            senseLabel = "Touch", itemCount = 5,
            instruction = "Touch 5 items near you and say their names out loud if you can.\nA part of your body is fine.",
            buttonLabel = "I touched it", summaryLead = "You touched:",
        },
        new SensePhase
        {
            senseLabel = "Sight", itemCount = 4,
            instruction = "Look around and name 4 things you can see.\nAnything counts, big or small.",
            buttonLabel = "I see it", summaryLead = "You saw:",
        },
        new SensePhase
        {
            senseLabel = "Hearing", itemCount = 3,
            instruction = "Listen for a moment and name 3 sounds you can hear.\nQuiet sounds count too.",
            buttonLabel = "I hear it", summaryLead = "You heard:",
        },
        new SensePhase
        {
            senseLabel = "Smell", itemCount = 2,
            instruction = "Name 2 things you can smell right now.\nIf nothing comes to you, 2 smells you love work just as well.",
            buttonLabel = "I smell it", summaryLead = "You smelled:",
        },
        new SensePhase
        {
            senseLabel = "Taste", itemCount = 1,
            instruction = "Name 1 thing you can taste.\nA favorite taste from memory is fine too.",
            buttonLabel = "I taste it", summaryLead = "You tasted:",
        },
    };

    [Header("Panels")]
    [SerializeField] private GameObject orientationPanel;
    [SerializeField] private GameObject touchPanel;
    [SerializeField] private GameObject completePanel;

    [Header("Orientation Panel References")]
    [SerializeField] private TextMeshProUGUI orientationQuestionText;
    [SerializeField] private TextMeshProUGUI orientationProgressText;
    [SerializeField] private TMP_InputField orientationAnswerInputField;

    // Field names kept from the touch-only version so existing scene wiring survives.
    // The one panel is reused for every sense phase.
    [Header("Sense Panel References")]
    [SerializeField] private TextMeshProUGUI touchInstructionText;
    [SerializeField] private TextMeshProUGUI touchProgressText;
    [SerializeField] private TMP_InputField itemInputField;
    [SerializeField] private TextMeshProUGUI logButtonLabel; // optional; label updates per sense when wired

    [Header("Complete Panel References")]
    [SerializeField] private TextMeshProUGUI summaryText;

    private int orientationIndex;
    private int phaseIndex;
    private int loggedCount;
    private readonly List<string> orientationAnswers = new();
    private readonly List<List<string>> loggedItems = new();

    private void OnEnable()
    {
        orientationIndex = 0;
        phaseIndex = 0;
        loggedCount = 0;
        orientationAnswers.Clear();
        loggedItems.Clear();
        foreach (SensePhase _ in sensePhases)
            loggedItems.Add(new List<string>());

        orientationPanel.SetActive(true);
        touchPanel.SetActive(false);
        completePanel.SetActive(false);

        ShowOrientationQuestion();
    }

    private void ShowOrientationQuestion()
    {
        orientationQuestionText.text = orientationQuestions[orientationIndex];
        orientationProgressText.text = $"{orientationIndex + 1} / {orientationQuestions.Length}";
        if (orientationAnswerInputField) orientationAnswerInputField.text = string.Empty;
    }

    // Wire the orientation panel's Next button to this. No auto-advance - the player moves at their own pace.
    public void AdvanceOrientation()
    {
        string answer = orientationAnswerInputField ? orientationAnswerInputField.text.Trim() : string.Empty;
        if (!string.IsNullOrEmpty(answer))
            orientationAnswers.Add(answer);

        orientationIndex++;
        if (orientationIndex >= orientationQuestions.Length)
        {
            StartSensePhases();
        }
        else
        {
            ShowOrientationQuestion();
        }
    }

    private void StartSensePhases()
    {
        orientationPanel.SetActive(false);
        touchPanel.SetActive(true);
        ShowSensePhase();
    }

    private void ShowSensePhase()
    {
        SensePhase phase = sensePhases[phaseIndex];
        touchInstructionText.text = phase.instruction;
        touchProgressText.text = $"{phase.senseLabel}   {loggedCount} / {phase.itemCount}";
        if (logButtonLabel) logButtonLabel.text = phase.buttonLabel;
        if (itemInputField) itemInputField.text = string.Empty;
    }

    // Wire the sense panel's confirm button to this. Player presses it once per item, at their own pace.
    // Name kept from the touch-only version so the existing button wiring survives.
    public void LogTouch()
    {
        SensePhase phase = sensePhases[phaseIndex];
        string item = itemInputField ? itemInputField.text.Trim() : string.Empty;
        loggedItems[phaseIndex].Add(string.IsNullOrEmpty(item) ? $"Item {loggedCount + 1}" : item);
        loggedCount++;

        if (loggedCount >= phase.itemCount)
        {
            AdvanceSensePhase();
        }
        else
        {
            ShowSensePhase();
        }
    }

    // Optionally wire a Skip button to this, for senses a player can't use right now.
    public void SkipSense()
    {
        AdvanceSensePhase();
    }

    private void AdvanceSensePhase()
    {
        phaseIndex++;
        loggedCount = 0;
        if (phaseIndex >= sensePhases.Length)
        {
            ShowComplete();
        }
        else
        {
            ShowSensePhase();
        }
    }

    private void ShowComplete()
    {
        touchPanel.SetActive(false);
        completePanel.SetActive(true);

        if (summaryText)
        {
            StringBuilder sb = new();
            if (orientationAnswers.Count > 0)
            {
                sb.AppendLine("You oriented yourself with:");
                sb.AppendLine(string.Join(", ", orientationAnswers));
                sb.AppendLine();
            }
            for (int i = 0; i < sensePhases.Length; i++)
            {
                if (loggedItems[i].Count == 0) continue;
                sb.AppendLine(sensePhases[i].summaryLead);
                sb.AppendLine(string.Join(", ", loggedItems[i]));
                sb.AppendLine();
            }
            sb.AppendLine("Carry this grounded feeling with you today.");
            summaryText.text = sb.ToString();
        }
    }
}
