using System.Collections;
using System.Collections.Generic;
using Enemy;
using Player;
using UI.Menus;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class EnemiesManager : MonoBehaviour
    {
        [SerializeField] private InGameMenu inGameMenu;
        [SerializeField] private Transform bulletsParent;
        [SerializeField] private Transform enemiesParent;
        [SerializeField] private List<EnemyAI> enemiesList;

        #region Constants

        private const float SpawnFrequency = 1.5f;
        private const int LevelDifficultyInitialVal = 4;
        private const int DifficultyCoeff = 1;

        #endregion

        private int _levelDifficulty;
        private int _currentLevelDifficulty;
        private int _enemyKilled;
        private readonly List<EnemyAI> _spawnedEnemies = new List<EnemyAI>();

        private void Awake()
        {
            EventManager.StartListening(Constants.StartGame, StartGame);
            EventManager.StartListening(Constants.NextLevel, StartGame);
            EventManager.StartListening(Constants.EnemyDied, CheckLevelProgress);
            EventManager.StartListening(Constants.GoHome, DestroyEnemies);
        }

        private void StartGame()
        {
            _currentLevelDifficulty = 0;
            DestroyEnemies();
            CalсulateLevelDifficulty();
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (_levelDifficulty > _currentLevelDifficulty)
            {
                if (!PauseManager.IsPause)
                    SpawnEnemy();
                yield return new WaitForSeconds(SpawnFrequency);
            }

            StopCoroutine(SpawnEnemies());
        }

        private void SpawnEnemy()
        {
            int m = Random.Range(0, enemiesList.Count);
            EnemyAI enemy = Instantiate(enemiesList[m], CalculateSpawnPosition(), Quaternion.identity, enemiesParent);
            enemy.Init(bulletsParent);
            _spawnedEnemies.Add(enemy);
            _currentLevelDifficulty += 1;
        }

        private void DestroyEnemies()
        {
            foreach (var t in _spawnedEnemies)
            {
                if (t != null)
                {
                    var spawnedEnemy = t;
                    Destroy(spawnedEnemy.gameObject);
                }
            }

            _spawnedEnemies.Clear();
            _enemyKilled = 0;
        }

        private void CalсulateLevelDifficulty()
        {
            _levelDifficulty = LevelDifficultyInitialVal + DifficultyCoeff * PlayerScores.ExpansionLevel;
            inGameMenu.Init(_levelDifficulty);
        }

        private void CheckLevelProgress()
        {
            _enemyKilled++;
            if (_levelDifficulty == _enemyKilled)
            {
                EventManager.SendEvent(Constants.LevelCompleted);
                _enemyKilled = 0;
            }
        }

        private Vector3 CalculateSpawnPosition()
        {
            Vector3 result;
            Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            bool leftOrRightX = (Random.value > 0.5f);
            if (leftOrRightX)
                result = new Vector3(Random.Range(0, screenBounds.x / 2), Random.Range(screenBounds.y / 2, screenBounds.y), 0);
            else
                result = new Vector3(Random.Range(screenBounds.x / 2, screenBounds.x), Random.Range(screenBounds.y / 2, screenBounds.y), 0);
            return result;

        }
    }
}
