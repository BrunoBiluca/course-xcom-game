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
                SetupView(unit, view);
            }
        }

        private static void SetupView(TrooperUnit unit, Transform view)
        {
            view.Setup<Image>(
                "portrait_holder.portrait",
                i => i.sprite = unit.UnitConfigTemplate.Portrait
            );

            view.Setup<TextMeshProUGUI>("container.name", t => t.text = unit.Name);

            view.Setup<TextMeshProUGUI>(
                "container.action_points.value",
                t => t.text = unit.Actor.ActionPoints.MaxAmount.ToString()
            );

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
        }
    }
}
