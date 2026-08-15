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
        "United States: call or text 988 to reach the Suicide & Crisis Lifeline, any time, day or night.\n\n" +
        "Outside the US: visit findahelpline.com, or search for \"crisis line\" and your country's name to find a local number.\n\n" +
        "If you are in immediate danger, call your local emergency number (911 in the US).\n\n" +
        "This app is a self-care companion. It is not a substitute for professional mental health care.";

    private void Awake()
    {
        if (helpText) helpText.text = helpMessage;
    }

    public void ToggleHelpPanel()
    {
        if (helpPanel) helpPanel.SetActive(!helpPanel.activeSelf);
    }
}
