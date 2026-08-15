using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SelfFriend : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject issuePanel;
    [SerializeField] private GameObject friendAdvicePanel;
    [SerializeField] private GameObject reflectionPanel;
    [SerializeField] private GameObject closingPanel;

    [Header("Issue Panel References")]
    [SerializeField] private TextMeshProUGUI issuePromptText;
    [SerializeField] private string issuePrompt = "What's something you're struggling with right now?";
    [SerializeField] private TMP_InputField issueInputField;

    [Header("Friend Advice Panel References")]
    [SerializeField] private TextMeshProUGUI friendPromptText;
    [SerializeField] private string friendPromptFormat = "Now imagine a friend came to you with this same problem:\n\"{0}\"\nWhat would you say to them?";
    [SerializeField] private string genericIssuePlaceholder = "what you're going through";
    [SerializeField] private TMP_InputField friendInputField;

    [Header("Reflection Panel References")]
    [SerializeField] private TextMeshProUGUI reflectionIssueText;
    [SerializeField] private string reflectionIssueFormat = "\"{0}\"\nWould you give this same advice to yourself?";
    // Yes / No buttons for that question live on this panel in the editor.

    [Header("Closing Panel References")]
    [SerializeField] private TextMeshProUGUI closingText;
    [TextArea][SerializeField] private string closingMessageYes = "Keep that same advice in mind as you sit with this problem. You deserve the compassion you'd give a friend.";
    [TextArea][SerializeField] private string closingMessageNo = "Notice that difference. The advice you'd give a friend is worth giving yourself too, even if it takes practice.";

    [SerializeField] private UnityEvent onComplete;

    private string currentIssueDisplay;
    private string currentAdviceDisplay;

    private void OnEnable()
    {
        if (issueInputField) issueInputField.text = string.Empty;
        if (friendInputField) friendInputField.text = string.Empty;
        currentIssueDisplay = string.Empty;
        currentAdviceDisplay = string.Empty;
        if (issuePromptText) issuePromptText.text = issuePrompt;

        issuePanel.SetActive(true);
        friendAdvicePanel.SetActive(false);
        reflectionPanel.SetActive(false);
        closingPanel.SetActive(false);
    }

    public void ContinueFromIssue()
    {
        string issue = issueInputField ? issueInputField.text.Trim() : string.Empty;
        currentIssueDisplay = string.IsNullOrEmpty(issue) ? genericIssuePlaceholder : issue;
        friendPromptText.text = string.Format(friendPromptFormat, currentIssueDisplay);

        issuePanel.SetActive(false);
        friendAdvicePanel.SetActive(true);
    }

    public void FinishGivingAdvice()
    {
        string advice = friendInputField ? friendInputField.text.Trim() : string.Empty;
        currentAdviceDisplay = advice;

        if (reflectionIssueText) reflectionIssueText.text = string.Format(reflectionIssueFormat, currentAdviceDisplay);

        friendAdvicePanel.SetActive(false);
        reflectionPanel.SetActive(true);
    }

    public void AnswerReflectionYes()
    {
        ShowClosing(closingMessageYes);
    }

    public void AnswerReflectionNo()
    {
        ShowClosing(closingMessageNo);
    }

    private void ShowClosing(string message)
    {
        closingText.text = message;
        reflectionPanel.SetActive(false);
        closingPanel.SetActive(true);
        onComplete?.Invoke();
    }
}
