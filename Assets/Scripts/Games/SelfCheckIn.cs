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

    [SerializeField]
    private CheckInQuestion[] questions = new CheckInQuestion[]
    {
        new CheckInQuestion { questionText = "Are you cold?", discomfortLabel = "Cold", suggestion = "Grab a blanket or layer up if you can." },
        new CheckInQuestion { questionText = "Are you hot?", discomfortLabel = "Hot", suggestion = "Turn on a fan or have a glass of cold water." },
        new CheckInQuestion { questionText = "Are you hungry?", discomfortLabel = "Hungry", suggestion = "Have a snack or plan your next meal." },
        new CheckInQuestion { questionText = "Are you thirsty?", discomfortLabel = "Thirsty", suggestion = "Drink some water." },
        new CheckInQuestion { questionText = "Are you tired?", discomfortLabel = "Tired", suggestion = "Rest if you can, even for a few minutes." },
        new CheckInQuestion { questionText = "Do you need to use the restroom?", discomfortLabel = "Restroom", suggestion = "Take a break to go." },
        new CheckInQuestion { questionText = "Are you in any physical pain?", discomfortLabel = "In pain", suggestion = "See what you can do to aleve the pain and see a doctor if needed." },
        new CheckInQuestion { questionText = "Are you feeling overwhelmed?", discomfortLabel = "Overwhelmed", suggestion = "Try a grounding or breathing exercise." },
        new CheckInQuestion { questionText = "Are you feeling lonely?", discomfortLabel = "Lonely", suggestion = "Try reaching out to someone you trust and say hi." },
    };

    [Header("Panels")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject resultsPanel;

    [Header("Question Panel References")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Results Panel References")]
    [SerializeField] private TextMeshProUGUI resultsText;
    [SerializeField] private string noFlagsMessage = "It seems like you're doing well. Enjoy your day.";

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
                sb.AppendLine($"- {question.suggestion}");
            }
            resultsText.text = sb.ToString();
        }

        onComplete?.Invoke();
    }
}
