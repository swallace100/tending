using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    public GameObject container;
    public GameObject optionsMenu;
    public GameObject creditsMenu;

    [SerializeField] private EventsHandler eventsHandler;
    [SerializeField] private GameObject quitButton;

    private void Start()
    {
        if (eventsHandler) eventsHandler.PlayMusic();
        if (eventsHandler) eventsHandler.PlayAmbianceMusic();

#if UNITY_WEBGL
        if (quitButton) quitButton.SetActive(false);
#endif
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void CreditsMenu()
    {
        creditsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
