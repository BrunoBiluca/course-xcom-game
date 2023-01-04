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

                var healthController = new HealthSystemController(unit.HealthSystem);
                healthController
                    .AddHealthBar(
                        view.FindComponent<IHealthBar>("container.health_bar_holder.health_bar")
                    )
                    .AddDiedView(
                        view
                            .FindTransform("container.health_bar_holder.died_icon")
                            .gameObject.Decorate()
                    );

                var actionPoints = view
                    .FindComponent<TextMeshProUGUI>("container", "action_points", "value");
                actionPoints.text = unit.Actor.ActionPoints.MaxAmount.ToString();
            }
        }
    }
}
