using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SelfCheckIn : MonoBehaviour
{
    [Serializable]
    public class CheckInQuestion
    {
        public string questionText;
        public string discomfortLabel;
        [TextArea] public string suggestion;
    }

    [SerializeField] private CheckInQuestion[] questions;

    [Header("Panels")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject resultsPanel;

    [Header("Question Panel References")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Results Panel References")]
    [SerializeField] private TextMeshProUGUI resultsText;
    [SerializeField] private string noFlagsMessage = "Nothing stood out. You seem to be doing okay right now.";

    [SerializeField] private UnityEvent onComplete;

    private int currentIndex;
    private readonly List<CheckInQuestion> flagged = new();

    private void OnEnable()
    {
        currentIndex = 0;
        flagged.Clear();
        resultsPanel.SetActive(false);
        questionPanel.SetActive(true);
        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        CheckInQuestion question = questions[currentIndex];
        questionText.text = question.questionText;
        progressText.text = $"{currentIndex + 1} / {questions.Length}";
    }

    public void AnswerYes()
    {
        flagged.Add(questions[currentIndex]);
        Advance();
    }

    public void AnswerNo()
    {
        Advance();
    }

    public void AnswerIDontKnow()
    {
        Advance();
    }

    private void Advance()
    {
        currentIndex++;
        if (currentIndex >= questions.Length)
        {
            ShowResults();
        }
        else
        {
            ShowCurrentQuestion();
        }
    }

    private void ShowResults()
    {
        questionPanel.SetActive(false);
        resultsPanel.SetActive(true);

        if (flagged.Count == 0)
        {
            resultsText.text = noFlagsMessage;
        }
        else
        {
            StringBuilder sb = new();
            foreach (CheckInQuestion question in flagged)
            {
                sb.AppendLine($"- {question.discomfortLabel}: {question.suggestion}");
            }
            resultsText.text = sb.ToString();
        }

        onComplete?.Invoke();
    }
}
