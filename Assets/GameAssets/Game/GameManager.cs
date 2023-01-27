using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.UI;

namespace GameAssets
{
    public class GameManager
        : Singleton<GameManager>
        , IDependencySetup<UnitsManager, EnemiesManager>
    {
        [SerializeField] private GameOverMenu gameOverMenu;
        public event Action OnPlayerWon;
        public event Action OnPlayerLost;

        private UnitsManager unitsManager;
        private EnemiesManager enemiesManager;

        public void Setup(
            UnitsManager unitsManager,
            EnemiesManager enemiesManager
        )
        {
            this.unitsManager = unitsManager;
            this.enemiesManager = enemiesManager;
        }

        public void StartGame()
        {
            unitsManager.OnAllUnitsDied += FinishWithLoserPlayer;
            enemiesManager.OnEnemiesDied += FinishWithWinnerPlayer;
        }

        private void FinishWithWinnerPlayer()
        {
            OnPlayerWon?.Invoke();
            gameOverMenu.Show("Jogador ganhou");
        }

        private void FinishWithLoserPlayer()
        {
            OnPlayerLost?.Invoke();
            gameOverMenu.Show("Jogador perdeu");
        }
    }
}
