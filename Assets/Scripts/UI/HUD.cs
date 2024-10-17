using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private Label scoreLabel;
    private Button pauseButton, resumeButton, quitButton, restartButton, retryButton, deathExitButton;
    private VisualElement pauseMenu;
    private VisualElement deathPopup;
    private int score;
    private bool isPaused;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        scoreLabel = root.Q<Label>("score-label");
        pauseButton = root.Q<Button>("pause-button");
        deathPopup = root.Q<VisualElement>("death-popup");
        pauseMenu = root.Q<VisualElement>("pause-menu");
        resumeButton = root.Q<Button>("resume-button");
        restartButton = root.Q<Button>("restart-button");
        quitButton = root.Q<Button>("quit-button");
        retryButton = root.Q<Button>("retry-button");
        deathExitButton = root.Q<Button>("death-quit-button");
        // callBack
        pauseButton.clicked += OnPauseButtonClicked;
        resumeButton.clicked += OnResumeButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;
        restartButton.clicked += OnRestartButtonClicked;
        retryButton.clicked += OnRestartButtonClicked;
        deathExitButton.clicked += OnQuitButtonClicked;
        score = 0;
        isPaused = false;
    }

    private void OnRestartButtonClicked()
    {
        Time.timeScale = 1;
        GameManager.Instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateScore(int newScore)
    {
        scoreLabel.text = $"Score: {newScore}";
    }

    private void OnQuitButtonClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void OnResumeButtonClicked()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.style.display = DisplayStyle.None;
    }

    private void OnPauseButtonClicked()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1; // Dừng hoặc tiếp tục game
        pauseMenu.style.display = isPaused ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void ShowDeadPopup()
    {
        deathPopup.style.display = DisplayStyle.Flex;
    }
}