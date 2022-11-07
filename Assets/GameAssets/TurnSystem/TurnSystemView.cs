using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;

namespace GameAssets
{
    public class TurnSystemView : MonoBehaviour
    {
        private ITurnSystem turnSystem;
        private TextMeshProUGUI text;
        private Button endTurnButton;
        private GameObjectVisibilityMono enemyTurnDisplay;

        public void Awake()
        {
            text = transform.FindComponent<TextMeshProUGUI>("turn_text");

            endTurnButton = transform
                .FindComponent<Button>("end_turn_button");
            endTurnButton.gameObject.AddComponent<GameObjectVisibilityMono>().Show();

            endTurnButton.onClick.AddListener(EndPlayerTurn);

            enemyTurnDisplay = transform
                .FindComponent<GameObjectVisibilityMono>("enemy_turn_display");
        }

        public void Setup(ITurnSystem turnSystem)
        {
            this.turnSystem = turnSystem;

            turnSystem.OnPlayerTurnEnded += UpdateTurnView;
            UpdateTurnView();

            turnSystem.OnEnemyTurnEnded += EndEnemyTurn;
        }

        private void EndPlayerTurn()
        {
            turnSystem.EndPlayerTurn();

            endTurnButton.GetComponent<GameObjectVisibilityMono>().Hide();
            enemyTurnDisplay.Show();
        }

        private void EndEnemyTurn()
        {
            turnSystem.EndPlayerTurn();

            endTurnButton.GetComponent<GameObjectVisibilityMono>().Show();
            enemyTurnDisplay.Hide();
        }

        private void UpdateTurnView()
        {
            text.text = $"Turn: {turnSystem.CurrentTurn}";
        }
    }
}
