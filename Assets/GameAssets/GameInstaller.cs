using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class GameInstaller : Singleton<GameInstaller>, IPrettyable
    {
        public event Action OnInstallerFinish;

        [SerializeField] private GameBinder binder;

        public UnitWorldGridManager GridManager { get; private set; }

        public IDependencyContainer Container { get; private set; }

        protected override void OnAwake()
        {
            binder.OnBinderFinish += StartInstaller;
        }

        private void StartInstaller()
        {
            Debug.Log("Start GameInstaller");

            Container = binder.Container;
            Container.RegisterAction<IBilucaLoggable>(b => b.Logger = UnityDebug.I);

            var selectableVisibility = new SelectableVisibilityHandler(
                Container.Resolve<UnitActionsView>().gameObject.Decorate(),
                Container.Resolve<ActionPointsView>().gameObject.Decorate()
            );
            selectableVisibility.Hide();

            FindObjectOfType<TurnSystemView>().Setup(Container.Resolve<ITurnSystem>());

            GridManager = Container.Resolve<UnitWorldGridManager>();
            // Events
            var unitSelection = Container.Resolve<UnitSelectionMono>();
            unitSelection.OnUnitUnselected += () => GridManager.ResetValidation();
            unitSelection.OnUnitSelected += selectableVisibility.Show;
            unitSelection.OnUnitUnselected += selectableVisibility.Hide;

            Container
                .Resolve<IActionSelector<IAPIntent>>()
                .OnActionUnselected += () => GridManager.ResetValidation();

            // Add units
            foreach(var unit in FindObjectsOfType<MonoBehaviour>().OfType<IUnit>())
            {
                GridManager.Add(unit);
            }

            Container.Resolve<ITurnSystem>()
                .OnPlayerTurnEnded += () => unitSelection.UnselectUnit();

            OnInstallerFinish?.Invoke();

            Container.Resolve<WorldGridView>().Display();
            Container.Resolve<EnemiesManager>().InstantiateUnits();
            Container.Resolve<UnitsManager>().InstantiateUnits();
            Container.Resolve<UnitsView>().Display();
            Container.Resolve<GameManager>().StartGame();
            Debug.Log("Finish GameInstaller");
        }

        public PrettyObject BePretty()
        {
            var installerColor = new Color(.38f, .35f, .06f);
            return new PrettyObject(false, installerColor, Color.white, gameObject);
        }
    }
}
