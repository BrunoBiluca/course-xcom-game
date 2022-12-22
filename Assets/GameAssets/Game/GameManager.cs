using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.UI;

namespace GameAssets
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameOverMenu gameOverMenu;
        public event Action OnPlayerWon;
        public event Action OnPlayerLost;

        public void Setup(
            UnitsManager unitsManager,
            EnemiesManager enemiesManager
        )
        {
            unitsManager.OnUnitsDied += FinishWithLoserPlayer;
            enemiesManager.OnEnemiesDied += FinishWithWinnerPlayer;
        }

        public void FinishWithWinnerPlayer()
        {
            OnPlayerWon?.Invoke();
            gameOverMenu.Show("Jogador ganhou");
        }

        public void FinishWithLoserPlayer()
        {
            OnPlayerLost?.Invoke();
            gameOverMenu.Show("Jogador perdeu");
        }
    }
}
