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
using UnityFoundation.WorldCursors;

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

            var turnSystemView = FindObjectOfType<TurnSystemView>();
            turnSystemView.Setup(Container.Resolve<ITurnSystem>());

            var visibilityHandler = FindObjectOfType<VisibilityHandlerSingleton>();
            visibilityHandler.Add(Container.Resolve<UnitActionsView>().gameObject.Decorate());
            visibilityHandler.Add(Container.Resolve<ActionPointsView>().gameObject.Decorate());
            visibilityHandler.Hide();

            GridManager = Container.Resolve<UnitWorldGridManager>();
            // Events
            var unitSelection = Container.Resolve<UnitSelectionMono>();
            unitSelection.OnUnitUnselected += () => GridManager.ResetValidation();
            unitSelection.OnUnitSelected += visibilityHandler.Show;
            unitSelection.OnUnitUnselected += visibilityHandler.Hide;

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

            Container.Resolve<IWorldCursor>().Enable();
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
