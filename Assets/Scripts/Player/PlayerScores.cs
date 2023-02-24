using Managers;

namespace Player
{
    public static class PlayerScores
    {
        private static float _playerScore;
        private static int _planetUpgradesCount;
        private static int _expansionLevel;

        public static float PlayerScore
        {
            get => _playerScore;
            set
            {
                _playerScore = value;
                EventManager.SendEvent(Constants.UpdateScore);
            }
        }

        public static int ExpansionLevel
        {
            get => _expansionLevel;
            set
            {
                _expansionLevel = value;
            }
        }

        public static void AddScore(float score)
        {
            PlayerScore += score;
        }

        public static void SubtractScore(float score)
        {
            PlayerScore -= score;
        }
    }
}
