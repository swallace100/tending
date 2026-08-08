using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GroundingExercise : MonoBehaviour
{
    [Header("Orientation Questions")]
    [SerializeField]
    private string[] orientationQuestions = new string[]
    {
        "What year is it?",
        "What season is it?",
        "What month is it?",
    };

    [Header("Touch Prompt")]
    [SerializeField] private int itemsToTouch = 5;
    [TextArea]
    [SerializeField]
    private string touchInstruction =
        "Touch something near you and say its name out loud if you can.\nIf nothing is nearby, you can touch a body part instead.";

    [Header("Panels")]
    [SerializeField] private GameObject orientationPanel;
    [SerializeField] private GameObject touchPanel;
    [SerializeField] private GameObject completePanel;

    [Header("Orientation Panel References")]
    [SerializeField] private TextMeshProUGUI orientationQuestionText;
    [SerializeField] private TextMeshProUGUI orientationProgressText;

    [Header("Touch Panel References")]
    [SerializeField] private TextMeshProUGUI touchInstructionText;
    [SerializeField] private TextMeshProUGUI touchProgressText;
    [SerializeField] private TMP_InputField itemInputField;

    [Header("Complete Panel References")]
    [SerializeField] private TextMeshProUGUI summaryText;

    [SerializeField] private UnityEvent onComplete;

    private int orientationIndex;
    private int touchedCount;
    private readonly List<string> touchedItems = new();

    private void OnEnable()
    {
        orientationIndex = 0;
        touchedCount = 0;
        touchedItems.Clear();

        orientationPanel.SetActive(true);
        touchPanel.SetActive(false);
        completePanel.SetActive(false);

        ShowOrientationQuestion();
    }

    private void ShowOrientationQuestion()
    {
        orientationQuestionText.text = orientationQuestions[orientationIndex];
        orientationProgressText.text = $"{orientationIndex + 1} / {orientationQuestions.Length}";
    }

    // Wire the orientation panel's Next button to this. No auto-advance - the player moves at their own pace.
    public void AdvanceOrientation()
    {
        orientationIndex++;
        if (orientationIndex >= orientationQuestions.Length)
        {
            StartTouchPhase();
        }
        else
        {
            ShowOrientationQuestion();
        }
    }

    private void StartTouchPhase()
    {
        orientationPanel.SetActive(false);
        touchPanel.SetActive(true);
        touchInstructionText.text = touchInstruction;
        ShowTouchProgress();
    }

    private void ShowTouchProgress()
    {
        touchProgressText.text = $"{touchedCount} / {itemsToTouch}";
        if (itemInputField) itemInputField.text = string.Empty;
    }

    // Wire the touch panel's button ("I touched it") to this. Player presses it once per item, at their own pace.
    public void LogTouch()
    {
        string item = itemInputField ? itemInputField.text.Trim() : string.Empty;
        touchedItems.Add(string.IsNullOrEmpty(item) ? $"Item {touchedCount + 1}" : item);
        touchedCount++;

        if (touchedCount >= itemsToTouch)
        {
            ShowComplete();
        }
        else
        {
            ShowTouchProgress();
        }
    }

    private void ShowComplete()
    {
        touchPanel.SetActive(false);
        completePanel.SetActive(true);

        if (summaryText)
        {
            StringBuilder sb = new();
            sb.AppendLine("You grounded yourself with:");
            foreach (string item in touchedItems)
                sb.AppendLine($"- {item}");
            summaryText.text = sb.ToString();
        }

        onComplete?.Invoke();
    }
}
