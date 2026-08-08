using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RoutineBuilder : MonoBehaviour
{
    [Serializable]
    public class RoutinePrompt
    {
        public string label;
        [TextArea] public string promptText;
    }

    [SerializeField]
    private RoutinePrompt[] prompts = new RoutinePrompt[]
    {
        new RoutinePrompt { label = "Diet", promptText = "Does your diet feel healthy to you?" },
        new RoutinePrompt { label = "Hydration", promptText = "Are you drinking enough water?" },
        new RoutinePrompt { label = "Sleep", promptText = "Are you getting enough sleep?" },
        new RoutinePrompt { label = "Movement", promptText = "What helps you feel physically active or moving?" },
        new RoutinePrompt { label = "Joy", promptText = "What brings you joy?" },
        new RoutinePrompt { label = "Relaxation", promptText = "What helps you relax?" },
        new RoutinePrompt { label = "Support People", promptText = "Who are some people you can talk to or reach out to, just to chat?" },
    };

    [Header("Panels")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private GameObject summaryPanel;

    [Header("Prompt Panel References")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TMP_InputField answerInputField;

    [Header("Summary Panel References")]
    [SerializeField] private TextMeshProUGUI summaryText;
    [SerializeField] private string emptySummaryMessage = "You didn't jot anything down this time - that's okay, you can always come back and try again.";

    [SerializeField] private UnityEvent onComplete;

    private int index;
    private readonly List<(string label, string answer)> collected = new();

    private void OnEnable()
    {
        index = 0;
        collected.Clear();

        promptPanel.SetActive(true);
        summaryPanel.SetActive(false);

        ShowPrompt();
    }

    private void ShowPrompt()
    {
        promptText.text = prompts[index].promptText;
        progressText.text = $"{index + 1} / {prompts.Length}";
        if (answerInputField) answerInputField.text = string.Empty;
    }

    // Wire the prompt panel's Next button to this. No auto-advance - the player moves at their own pace.
    public void AdvanceRoutine()
    {
        string answer = answerInputField ? answerInputField.text.Trim() : string.Empty;
        if (!string.IsNullOrEmpty(answer))
            collected.Add((prompts[index].label, answer));

        index++;
        if (index >= prompts.Length)
        {
            ShowSummary();
        }
        else
        {
            ShowPrompt();
        }
    }

    private void ShowSummary()
    {
        promptPanel.SetActive(false);
        summaryPanel.SetActive(true);

        if (collected.Count == 0)
        {
            summaryText.text = emptySummaryMessage;
        }
        else
        {
            StringBuilder sb = new();
            sb.AppendLine("Your routine, so far:");
            foreach ((string label, string answer) in collected)
                sb.AppendLine($"- {label}: {answer}");
            summaryText.text = sb.ToString();
        }

        onComplete?.Invoke();
    }
}
