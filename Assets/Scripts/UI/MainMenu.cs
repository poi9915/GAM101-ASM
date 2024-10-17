using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private UIDocument _document;
        private Button _playButton;
        private Button _exitButton;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _playButton = _document.rootVisualElement.Q<Button>("btnStart");
            _exitButton = _document.rootVisualElement.Q<Button>("btnExit");
            _playButton.clicked += PlayButtonClicked;
            _exitButton.clicked += ExitButtonClicked;
        }

        void PlayButtonClicked()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }

        void ExitButtonClicked()
        {
            Application.Quit();
        }
    }
}