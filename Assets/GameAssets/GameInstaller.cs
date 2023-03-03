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

            var turnSystemView = Container.Resolve<TurnSystemView>();
            var turnSystem = Container.Resolve<ITurnSystem>();
            turnSystemView.Setup(turnSystem);

            var gridManager = Container.Resolve<UnitWorldGridManager>();

            var unitSelection = Container.Resolve<UnitSelectionMono>();

            // Events
            turnSystem.OnPlayerTurnStarted += unitSelection.Enable;
            turnSystem.OnPlayerTurnEnded += unitSelection.Disable;

            Container.Resolve<IWorldCursor>().Enable();

            Container.Resolve<UnitWorldGridManager>().InitUnits();
            Container.Resolve<EnemiesManager>().InstantiateUnits();
            Container.Resolve<UnitsManager>().InstantiateUnits();

            Container.Resolve<ViewsManager>().Init();
            Container.Resolve<GameManager>().StartGame();

            unitSelection.Enable();

            OnInstallerFinish?.Invoke();
            Debug.Log("Finish GameInstaller");
        }

        public PrettyObject BePretty()
        {
            var installerColor = new Color(.38f, .35f, .06f);
            return new PrettyObject(false, installerColor, Color.white, gameObject);
        }
    }
}
