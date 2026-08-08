using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameSelectionMenu : MonoBehaviour
{
    [Serializable]
    public class GameOption
    {
        public string displayName;
        [TextArea] public string description;
        public string sceneName;
    }

    [SerializeField] private GameOption[] games;

    [Header("Panels")]
    [SerializeField] private GameObject gameButtonsPanel;
    [SerializeField] private GameObject descriptionPanel;

    [Header("Description Panel References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button startButton;

    private GameOption selectedGame;

    private void Awake()
    {
        startButton.onClick.AddListener(StartSelectedGame);
    }

    // Wire each game button's OnClick to SelectGame(index), matching the order of the games array.
    public void SelectGame(int index)
    {
        if (index < 0 || index >= games.Length) return;

        selectedGame = games[index];
        titleText.text = selectedGame.displayName;
        descriptionText.text = selectedGame.description;

        gameButtonsPanel.SetActive(false);
        descriptionPanel.SetActive(true);
    }

    public void BackToGameButtons()
    {
        descriptionPanel.SetActive(false);
        gameButtonsPanel.SetActive(true);
        selectedGame = null;
    }

    private void StartSelectedGame()
    {
        if (selectedGame == null || string.IsNullOrEmpty(selectedGame.sceneName)) return;
        SceneManager.LoadScene(selectedGame.sceneName);
    }
}
