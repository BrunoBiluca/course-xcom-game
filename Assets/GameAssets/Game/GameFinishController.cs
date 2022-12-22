using UnityFoundation.UI;

namespace GameAssets
{
    public class GameFinishController
    {
        private readonly GameOverMenu gameOverMenu;

        public GameFinishController(GameManager gameManager, GameOverMenu gameOverMenu)
        {
            gameManager.OnPlayerWon += HandleWinner;
            gameManager.OnPlayerLost += HandleLoser;
            this.gameOverMenu = gameOverMenu;
        }

        private void HandleWinner()
        {
            gameOverMenu.Show("Jogador ganhou");
        }

        private void HandleLoser()
        {
            gameOverMenu.Show("Jogador perdeu");
        }
    }
}
