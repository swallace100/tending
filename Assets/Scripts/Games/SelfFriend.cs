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
    [SerializeField] private TMP_InputField issueInputField;

    [Header("Friend Advice Panel References")]
    [SerializeField] private TextMeshProUGUI friendPromptText;
    [SerializeField] private string friendPromptFormat = "Now imagine a friend came to you with this same problem:\n\n\"{0}\"\n\nWhat would you say to them?";
    [SerializeField] private string genericIssuePlaceholder = "what you're going through";

    [Header("Reflection Panel References")]
    [SerializeField] private TextMeshProUGUI reflectionIssueText;
    [SerializeField] private string reflectionIssueFormat = "\"{0}\"\n\nWould you give this same advice to yourself?";
    // Yes / No buttons for that question live on this panel in the editor.

    [Header("Closing Panel References")]
    [SerializeField] private TextMeshProUGUI closingText;
    [TextArea][SerializeField] private string closingMessageYes = "Keep that same advice in mind as you sit with this problem. You deserve the compassion you'd give a friend.";
    [TextArea][SerializeField] private string closingMessageNo = "Notice that gap. The advice you'd give a friend is worth giving yourself too, even if it takes practice.";

    [SerializeField] private UnityEvent onComplete;

    private string currentIssueDisplay;

    private void OnEnable()
    {
        if (issueInputField) issueInputField.text = string.Empty;
        currentIssueDisplay = string.Empty;

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
        if (reflectionIssueText) reflectionIssueText.text = string.Format(reflectionIssueFormat, currentIssueDisplay);

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
