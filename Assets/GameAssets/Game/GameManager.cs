using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.UI;

namespace GameAssets
{
    public class GameManager
        : Singleton<GameManager>
        , IDependencySetup<UnitsManager, EnemiesManager, ICharacterSelector>
    {
        [SerializeField] private GameOverMenu gameOverMenu;
        public event Action OnPlayerWon;
        public event Action OnPlayerLost;

        private UnitsManager unitsManager;
        private EnemiesManager enemiesManager;
        private ICharacterSelector characterSelector;

        public void Setup(
            UnitsManager unitsManager,
            EnemiesManager enemiesManager,
            ICharacterSelector characterSelector
        )
        {
            this.unitsManager = unitsManager;
            this.enemiesManager = enemiesManager;
            this.characterSelector = characterSelector;
        }

        public void StartGame()
        {
            unitsManager.OnAllUnitsDied += FinishWithLoserPlayer;
            enemiesManager.OnEnemiesDied += FinishWithWinnerPlayer;
        }

        private void FinishWithWinnerPlayer()
        {
            AsyncProcessor.I.ExecuteWithDelay(2f, () => {
                characterSelector.UnselectUnit();
                ViewsManager.I.AllViewsHide();
                OnPlayerWon?.Invoke();
                gameOverMenu.Show("Jogador ganhou");
            });
        }

        private void FinishWithLoserPlayer()
        {
            AsyncProcessor.I.ExecuteWithDelay(2f, () => {
                characterSelector.UnselectUnit();
                ViewsManager.I.AllViewsHide();
                OnPlayerLost?.Invoke();
                gameOverMenu.Show("Jogador perdeu");
            });
        }
    }
}
