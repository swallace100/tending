using UnityEngine;
using TMPro;

// Attach anywhere persistent (e.g. the Canvas). Wire the "?" button's OnClick
// to ToggleHelpPanel. If helpText is wired, the default message below fills
// it, so the panel needs no authored copy - edit the message here or
// override it in the Inspector.
public class CrisisHelpMenu : MonoBehaviour
{
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private TextMeshProUGUI helpText;

    [TextArea(8, 16)]
    [SerializeField]
    private string helpMessage =
        "If you are in crisis or need support right now, you are not alone.\n\n" +
        "United States: Call or text 988 to reach the Suicide & Crisis Lifeline at any time.\n\n" +
        "Outside the US: Visit findahelpline.com or search for \"crisis line\" at your location.\n\n" +
        "If you are in immediate danger, please call your local emergency number (911 in the US).";

    private void Awake()
    {
        if (helpText) helpText.text = helpMessage;
    }

    public void ToggleHelpPanel()
    {
        if (helpPanel) helpPanel.SetActive(!helpPanel.activeSelf);
    }
}
