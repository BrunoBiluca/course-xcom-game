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

        public void Awake()
        {
            text = transform.FindComponent<TextMeshProUGUI>("turn_text");

            transform.FindComponent<Button>("end_turn_button").onClick.AddListener(EndTurn);
        }

        public void Setup(ITurnSystem turnSystem)
        {
            this.turnSystem = turnSystem;

            turnSystem.OnTurnEnded += UpdateTurnView;
            UpdateTurnView();
        }

        private void EndTurn()
        {
            turnSystem.EndTurn();
        }

        private void UpdateTurnView()
        {
            text.text = $"Turn: {turnSystem.CurrentTurn}";
        }
    }
}
