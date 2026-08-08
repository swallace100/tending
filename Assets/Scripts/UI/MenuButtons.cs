using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Panels")]
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject returnToMainMenuWarningPanel;

    public void OnOptionsButtonPressed()
    {
        optionsMenuPanel.SetActive(true);
    }

    public void OnHomeButtonPressed()
    {
        returnToMainMenuWarningPanel.SetActive(true);
    }

    public void OnCancelReturnToMainMenu()
    {
        returnToMainMenuWarningPanel.SetActive(false);
    }

    public void OnConfirmReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
