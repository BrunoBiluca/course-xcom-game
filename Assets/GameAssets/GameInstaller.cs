using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.Editor.Hierarchy;

namespace GameAssets
{
    public class GameInstaller : Singleton<GameInstaller>, IPrettyable
    {
        [Header("Config")]
        [SerializeField] private LevelSetupConfig levelSetupConfig;

        [Header("UI")]
        [SerializeField] private UnitActionSelectionView unitActionSelectionView;
        [SerializeField] private ActionPointsView actionPointsView;
        [SerializeField] private TurnSystemView turnSystemView;

        [Header("Grid")]
        [SerializeField] private GridWorldCursor worldCursor;
        [SerializeField] private GridXZMono grid;
        [SerializeField] private GridXZMonoDebug gridDebug;

        [Header("Managers")]
        [SerializeField] private UnitSelectionMono unitSelection;
        [SerializeField] private UnitsManager unitsManager;
        [SerializeField] private EnemiesManager enemiesManager;
        [SerializeField] private ProjectileFactory projectileFactory;
        private UnitWorldGridXZManager gridManager;

        [Header("Debug")]
        [SerializeField] private UnityDebug unityDebug;

        public PrettyObject BePretty()
        {
            var installerColor = new Color(.38f, .35f, .06f);
            return new PrettyObject(false, installerColor, Color.white, gameObject);
        }

        protected override void OnStart()
        {
            grid.Setup(levelSetupConfig.GridConfig);

            var raycastHandler = new RaycastHandler(new CameraDecorator(Camera.main));
            worldCursor.Setup(raycastHandler, grid.Grid);

            gridManager = new UnitWorldGridXZManager(grid.Grid);

            gridDebug.Setup(gridManager);

            var unitSelection = FindObjectOfType<UnitSelectionMono>();
            unitSelection.Setup(worldCursor);

            unitSelection.OnUnitDeselected += () => gridManager.ResetRangeValidation();

            var unitActionsFactory = new UnitActionsFactory(
                unitSelection,
                worldCursor,
                gridManager,
                projectileFactory
            );

            var unitActionHandler = new UnitActionHandler(unitSelection);

            unitActionSelectionView.Setup(unitActionHandler, unitActionsFactory);

            actionPointsView.Setup(unitSelection);

            var actorSelectorVisibilityHandler = new ActorSelectorVisibilityHandler(
                unitSelection,
                new GameObjectDecorator(actionPointsView.gameObject),
                new GameObjectDecorator(unitActionSelectionView.gameObject)
            );
            actorSelectorVisibilityHandler.Logger = unityDebug;
            actorSelectorVisibilityHandler.Hide();

            var turnSystem = new TurnSystem();

            unitsManager.Setup(levelSetupConfig, worldCursor, gridManager, turnSystem);

            enemiesManager.Logger = unityDebug;
            enemiesManager.Setup(levelSetupConfig, gridManager, turnSystem);

            turnSystemView.Setup(turnSystem);
        }

        public void Update()
        {
            gridManager.Update();
        }
    }
}
