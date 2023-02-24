using Extensions;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button closeSettingsButton;
        [SerializeField] private CanvasGroup settingsMenuCanvasGroup;
        [SerializeField] private CanvasGroup startMenuCanvasGroup;

        private void Awake()
        {
            playButton.onClick.AddListener(StartGame);
            settingsButton.onClick.AddListener(EnableSettingsMenu);
            closeSettingsButton.onClick.AddListener(DisableSettingsMenu);
            EventManager.StartListening(Constants.GoHome, Show);
        }

        private void StartGame()
        {
            Hide();
            PlayerScores.ExpansionLevel = 1;
            PlayerScores.PlayerScore = 0;
            EventManager.SendEvent(Constants.StartGame);
        }

        private void EnableSettingsMenu()
        {
            settingsMenuCanvasGroup.SetActive(true);
        }

        private void DisableSettingsMenu()
        {
            settingsMenuCanvasGroup.SetActive(false);
        }
    
        private void Show()
        {
            startMenuCanvasGroup.SetActive(true);
            EventManager.SendEvent(Constants.PauseGame);
        }

        private void Hide()
        {
            startMenuCanvasGroup.SetActive(false);
            EventManager.SendEvent(Constants.UnPauseGame);
        }
    }
}
