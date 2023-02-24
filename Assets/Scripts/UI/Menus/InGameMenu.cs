using Extensions;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;
        [SerializeField] private Button pauseButton;
        [SerializeField] private CanvasGroup canvasGroup;

        public void Init(int maxProgress)
        {
            progressBar.maxValue = maxProgress;
            progressBar.value = 0;
        }
        
        private void Awake()
        {
            pauseButton.onClick.AddListener(PauseClicked);
            EventManager.StartListening(Constants.EnemyDied, UpdateProgress);
            EventManager.StartListening(Constants.StartGame, Show);
            EventManager.StartListening(Constants.RevivePlayer, Show);
            EventManager.StartListening(Constants.PlayerDiedEvent, Hide);
            EventManager.StartListening(Constants.LevelCompleted, Hide);
            EventManager.StartListening(Constants.NextLevel, Show);
            EventManager.StartListening(Constants.GoHome, Hide);
        }

        private void UpdateProgress()
        {
            progressBar.value += 1;
        }

        private void PauseClicked()
        {
            EventManager.SendEvent(Constants.PauseGame);
        }
        
        private void Show()
        {
            canvasGroup.SetActive(true);
        }

        private void Hide()
        {
            canvasGroup.SetActive(false);
        }
    }
}