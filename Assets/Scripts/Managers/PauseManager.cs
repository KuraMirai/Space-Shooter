using UnityEngine;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        public static bool IsPause { get; private set; }

        private void OnEnable()
        {
            EventManager.StartListening(Constants.PlayerDiedEvent, Pause);
            EventManager.StartListening(Constants.LevelCompleted, Pause);
            EventManager.StartListening(Constants.StartGame, StartGame);
            EventManager.StartListening(Constants.RevivePlayer, UnPause);
            EventManager.StartListening(Constants.NextLevel, StartGame);
            EventManager.StartListening(Constants.PauseGame, Pause);
            EventManager.StartListening(Constants.UnPauseGame, StartGame);
        }

        private void Awake()
        {
            Pause();
        }

        private void OnDisable()
        {
            EventManager.StopListening(Constants.PlayerDiedEvent, Pause);
            EventManager.StopListening(Constants.LevelCompleted, Pause);
            EventManager.StopListening(Constants.StartGame, StartGame);
            EventManager.StopListening(Constants.RevivePlayer, UnPause);
            EventManager.StopListening(Constants.NextLevel, StartGame);
            EventManager.StopListening(Constants.PauseGame, Pause);
            EventManager.StopListening(Constants.UnPauseGame, StartGame);
        }

        public void Pause()
        {
            IsPause = true;
        }

        public void UnPause()
        {
            IsPause = false;
        }

        public void StartGame()
        {
            UnPause();
        }
    }
}
