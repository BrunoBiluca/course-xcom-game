using System;
using System.ComponentModel;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.TurnSystem;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public enum ProjectileFactories
    {
        Shoot,
        Grenade,
        Meteor
    }

    public class GameBinder : MonoBehaviour
    {
        public event Action OnBinderFinish;

        [Header("Config")]
        [SerializeField] private LevelSetupConfigSO levelSetupConfig;

        public IDependencyContainer Container { get; private set; }

        public void Awake()
        {
            GetComponent<GameSceneLoader>().OnAllScenesLoaded += SetupBinder;
        }

        private void SetupBinder()
        {
            Debug.Log("Start GameBinder");

            var binder = new DependencyBinder();

            binder.RegisterModule(new ActionsModule());
            binder.RegisterModule(new PlayerUnitIntentsModule());

            // Always create new instance
            binder.Register<ISelector, GridSelector>();
            binder.Register<IRaycastHandler, RaycastHandler>();
            binder.Register<IEnemyActionIntentFactory, EnemyActionIntentFactory>();

            // Constants or Singletons
            binder.Register(levelSetupConfig);
            binder.Register(levelSetupConfig.actionsConfig);

            binder.Register(GameManager.I);
            binder.Register<IAsyncProcessor>(AsyncProcessor.I);
            binder.Register<ICamera>(new CameraDecorator(Camera.main));

            var cursor = FindObjectOfType<UnitGridWorldCursor>();
            binder.Register<IWorldCursor>(cursor);
            binder.Register(cursor);

            var unitSelector = FindObjectOfType<UnitSelectionMono>();
            binder.Register<ICharacterSelector>(unitSelector);
            binder.Register<IActorSelector<ICharacterUnit>>(unitSelector);
            binder.Register(unitSelector);

            binder.Register(FindObjectOfType<UnitActionsView>());
            binder.Register(FindObjectOfType<ActionPointsView>());
            binder.Register(FindObjectOfType<PlayerInputsView>());

            binder.Register(FindObjectOfType<UnitsManager>());
            binder.Register(FindObjectOfType<EnemiesManager>());

            binder.Register(FindObjectOfType<WorldGridView>());
            binder.Register(FindObjectOfType<UnitsView>());

            UnitWorldGridXZ grid = FindObjectOfType<UnitWorldGridXZ>();
            binder.Register(grid);
            binder.Register(grid.Grid);

            binder.Register<IProjectileFactory>(
                FindObjectOfType<TransformProjectileFactory>(),
                ProjectileFactories.Shoot
            );
            binder.Register<IProjectileFactory>(
                FindObjectOfType<GrenadeProjectileFactory>(),
                ProjectileFactories.Grenade
            );
            binder.Register<IProjectileFactory>(
                FindObjectOfType<MeteorProjectileFactory>(),
                ProjectileFactories.Meteor
            );

            binder.RegisterSingleton<ITurnSystem, TurnSystem>();
            binder.RegisterSingleton<IActorSelector<IAPActor>, CharacterActorSelector>();
            binder.RegisterSingleton<IActionSelector<IAPIntent>, ActionSelector>();
            binder.RegisterSingleton<IUnitWorldGridManager, UnitWorldGridManager>();

            binder.Register(UnityDebug.I);

            Container = binder.Build();

            Debug.Log("Finish GameBinder");
            OnBinderFinish?.Invoke();
        }
    }
}
