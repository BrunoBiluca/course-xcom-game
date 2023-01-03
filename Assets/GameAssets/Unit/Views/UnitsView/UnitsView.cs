using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;
using UnityFoundation.UI.Components;

namespace GameAssets
{
    public class UnitsView : MonoBehaviour
    {
        [SerializeField] private Transform unitsViewHolder;
        [SerializeField] private GameObject unitViewPrefab;
        private UnitsManager unitsManager;

        public void Setup(UnitsManager unitsManager)
        {
            this.unitsManager = unitsManager;

            CreateView();
        }

        private void CreateView()
        {
            foreach(var unit in unitsManager.GetAllUnits())
            {
                var view = Instantiate(unitViewPrefab, unitsViewHolder).transform;

                var portrait = view.FindComponent<Image>("portrait");

                var name = view.FindComponent<TextMeshProUGUI>("container", "name");
                name.text = unit.Name;

                var healthbar = view.FindComponent<IHealthBar>("container", "health_bar");
                healthbar.Setup(unit.HealthSystem.BaseHealth);

                var actionPoints = view
                    .FindComponent<TextMeshProUGUI>("container", "action_points", "value");
                actionPoints.text = unit.Actor.ActionPoints.MaxAmount.ToString();
            }
        }
    }
}
