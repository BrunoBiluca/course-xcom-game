using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class GameInstaller : Singleton<GameInstaller>, IPrettyable
    {
        [Header("Config")]
        [SerializeField] private LevelSetupConfig levelSetupConfig;

        [Header("UI")]
        [SerializeField] private APUnitActionSelectionView unitActionSelectionView;
        [SerializeField] private ActionPointsView actionPointsView;
        [SerializeField] private TurnSystemView turnSystemView;

        [Header("Grid")]
        [SerializeField] private UnitGridWorldCursor worldCursor;
        [SerializeField] private UnitWorldGridXZ grid;
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

            var apUnitActionsFactory = new APUnitActionsFactory(unitActionsFactory);

            var unitActionHandler = new APUnitActionSelection(unitSelection);

            unitActionSelectionView.Setup(unitActionHandler, apUnitActionsFactory);

            actionPointsView.Setup(unitSelection);

            var selectableVisibility = new SelectableVisibilityHandler(
                new GameObjectDecorator(actionPointsView.gameObject),
                new GameObjectDecorator(unitActionSelectionView.gameObject)
            ) {
                Logger = unityDebug
            };
            selectableVisibility.Hide();

            unitSelection.OnUnitSelected += () => selectableVisibility.Show();
            unitSelection.OnUnitDeselected += () => selectableVisibility.Hide();

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
