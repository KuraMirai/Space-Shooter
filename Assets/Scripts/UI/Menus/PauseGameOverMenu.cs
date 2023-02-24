using Extensions;
using Managers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class PauseGameOverMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text playerScoreText;
        [SerializeField] private TMP_Text titleTextMeshPro;
        [SerializeField] private Button unPauseButton;
        [SerializeField] private Button playNextButton;
        [SerializeField] private Button reviveButton;
        [SerializeField] private Button homeButton;

        private void Awake()
        {
            reviveButton.onClick.AddListener(ReviveButtonPressed);
            unPauseButton.onClick.AddListener(OnUnPauseButtonPressed);
            playNextButton.onClick.AddListener(OnPlayNextPressed);
            homeButton.onClick.AddListener(OnHomePressed);
        }

        private void OnEnable()
        {
            EventManager.StartListening(Constants.PlayerDiedEvent, OnPlayerDied);
            EventManager.StartListening(Constants.UpdateScore, OnUpdateScore);
            EventManager.StartListening(Constants.LevelCompleted, OnLevelPassed);
            EventManager.StartListening(Constants.PauseGame, OnPause);
            EventManager.StartListening(Constants.GoHome, Hide);
        }
        
        private void OnDisable()
        {
            EventManager.StopListening(Constants.PlayerDiedEvent, OnPlayerDied);
            EventManager.StopListening(Constants.UpdateScore, OnUpdateScore);
            EventManager.StopListening(Constants.LevelCompleted, OnLevelPassed);
            EventManager.StopListening(Constants.PauseGame, OnPause);
            EventManager.StopListening(Constants.GoHome, Hide);
        }

        private void Show()
        {
            canvasGroup.SetActive(true);
        }

        private void Hide()
        {
            DeactivateAllButtons();
            canvasGroup.SetActive(false);
        }

        private void OnUpdateScore()
        {
            playerScoreText.text = PlayerScores.PlayerScore.ToString();
        }

        private void DeactivateAllButtons()
        {
            unPauseButton.gameObject.SetActive(false);
            playNextButton.gameObject.SetActive(false);
            reviveButton.gameObject.SetActive(false); 
        }

        private void OnPause()
        {
            titleTextMeshPro.text = "Pause";
            unPauseButton.gameObject.SetActive(true);
            Show();
        }

        private void OnLevelPassed()
        {
            titleTextMeshPro.text = "Level Passed";
            playNextButton.gameObject.SetActive(true);
            Show();
        }

        private void OnPlayerDied()
        {
            titleTextMeshPro.text = "Revive";
            reviveButton.gameObject.SetActive(true);
            Show();
        }

        private void ReviveButtonPressed()
        {
            Hide();
            EventManager.SendEvent(Constants.RevivePlayer);
        }

        private void OnUnPauseButtonPressed()
        {
            Hide();
            EventManager.SendEvent(Constants.UnPauseGame);
        }

        private void OnPlayNextPressed()
        {
            Hide();
            PlayerScores.ExpansionLevel++;
            EventManager.SendEvent(Constants.NextLevel);
        }
        
        private void OnHomePressed()
        {
            Hide();
            EventManager.SendEvent(Constants.GoHome);
        }
    }
}
