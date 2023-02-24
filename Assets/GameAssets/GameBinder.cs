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

            var cursor = FindObject<UnitGridWorldCursor>();
            binder.Register<IWorldCursor>(cursor);
            binder.Register(cursor);

            var unitSelector = FindObjectOfType<UnitSelectionMono>();
            binder.Register<ICharacterSelector>(unitSelector);
            binder.Register<IActorSelector<ICharacterUnit>>(unitSelector);
            binder.Register(unitSelector);

            binder.Register(FindObject<TurnSystemView>());
            binder.Register(FindObject<UnitIntentsView>());
            binder.Register(FindObject<ActionPointsView>());
            binder.Register(FindObject<PlayerInputsView>());

            binder.Register(FindObject<UnitsManager>());
            binder.Register(FindObject<EnemiesManager>());

            binder.Register(FindObject<WorldGridView>());
            binder.Register(FindObject<UnitsView>());

            UnitWorldGridXZ grid = FindObject<UnitWorldGridXZ>();
            binder.Register(grid);
            binder.Register(grid.Grid);

            binder.Register<IProjectileFactory>(
                FindObject<TransformProjectileFactory>(),
                ProjectileFactories.Shoot
            );
            binder.Register<IProjectileFactory>(
                FindObject<GrenadeProjectileFactory>(),
                ProjectileFactories.Grenade
            );
            binder.Register<IProjectileFactory>(
                FindObject<MeteorProjectileFactory>(),
                ProjectileFactories.Meteor
            );
            binder.Register<IProjectileFactory>(
                FindObject<ProjectileByTimeFactory>(),
                ProjectileFactories.WerewolfShot
            );

            binder.RegisterSingleton<ITurnSystem, TurnSystem>();
            binder.RegisterSingleton<IActorSelector<IAPActor>, CharacterActorSelector>();

            binder.RegisterSingleton<IGridIntentSelector, IntentSelector>();
            binder.Register<IGridIntentQuery, GridIntentQuery>();

            binder.RegisterSingleton<IUnitWorldGridManager, UnitWorldGridManager>();

            binder.Register(ViewsManager.I);
            binder.Register(UnityDebug.I);

            Container = binder.Build();

            Debug.Log("Finish GameBinder");
            OnBinderFinish?.Invoke();
        }

        private T FindObject<T>() where T : UnityEngine.Object
        {
            return FindObjectOfType<T>(includeInactive: true);
        }
    }
}
