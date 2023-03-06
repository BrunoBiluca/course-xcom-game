using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;
using UnityFoundation.ResourceManagement;

namespace GameAssets
{
    public class UnitsView : BaseView, IDependencySetup<UnitsManager>
    {
        [SerializeField] private Transform unitsViewHolder;
        [SerializeField] private GameObject unitViewPrefab;
        private UnitsManager unitsManager;

        private TextMeshProUGUI actionPointsText;

        public void Setup(UnitsManager unitsManager)
        {
            this.unitsManager = unitsManager;
        }

        protected override void OnFirstShow()
        {
            InstantiateUnitsView();
        }

        public void InstantiateUnitsView()
        {
            gameObject.SetActive(true);
            foreach(var unit in unitsManager.GetAllUnits())
            {
                var view = Instantiate(unitViewPrefab, unitsViewHolder).transform;
                SetupView(unit, view);
            }
        }

        private void SetupView(ICharacterUnit unit, Transform view)
        {
            view.Setup<Image>(
                "portrait_holder.portrait",
                i => i.sprite = unit.UnitConfig.Portrait
            );

            view.Setup<TextMeshProUGUI>("container.name", t => t.text = unit.Name);

            actionPointsText = view.FindComponent<TextMeshProUGUI>("container.action_points.value");
            HandleActionPointsChanged(unit.Actor.ActionPoints);
            unit.Actor.ActionPoints.OnResourceChanged += HandleActionPointsChanged;

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

        private void HandleActionPointsChanged(IResourceManager resourceManager)
        {
            var text = resourceManager.CurrentAmount.ToString();
            text += " / ";
            text += resourceManager.MaxAmount.ToString();
            actionPointsText.text = text;
        }
    }
}
