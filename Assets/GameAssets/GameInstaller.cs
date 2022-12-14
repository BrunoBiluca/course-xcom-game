using System.Linq;
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
        private UnitWorldGridManager gridManager;

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

            gridManager = new UnitWorldGridManager(grid.Grid);

            gridDebug.Setup(gridManager);

            var unitSelection = FindObjectOfType<UnitSelectionMono>();
            unitSelection.Setup(worldCursor);

            unitSelection.OnUnitUnselected += () => gridManager.ResetValidation();

            var unitActionsFactory = new UnitActionsFactory(
                unitSelection,
                worldCursor,
                gridManager,
                projectileFactory
            );

            var actionSelection = new APUnitActionSelection(unitSelection);

            actionSelection.OnActionUnselected += () => gridManager.ResetValidation();

            unitActionSelectionView.Setup(actionSelection, unitActionsFactory);

            actionPointsView.Setup(unitSelection);

            var selectableVisibility = new SelectableVisibilityHandler(
                new GameObjectDecorator(actionPointsView.gameObject),
                new GameObjectDecorator(unitActionSelectionView.gameObject)
            ) {
                Logger = unityDebug
            };
            selectableVisibility.Hide();

            unitSelection.OnUnitSelected += () => selectableVisibility.Show();
            unitSelection.OnUnitUnselected += () => selectableVisibility.Hide();

            var turnSystem = new TurnSystem();

            unitsManager.Setup(
                levelSetupConfig, worldCursor, gridManager, turnSystem, unitSelection
            );

            enemiesManager.Logger = unityDebug;
            enemiesManager.Setup(levelSetupConfig, gridManager, turnSystem);

            turnSystemView.Setup(turnSystem);

            foreach(var unit in FindObjectsOfType<MonoBehaviour>().OfType<IUnit>())
            {
                gridManager.Add(unit);
            }
        }

        public void Update()
        {
            gridManager.Update();
        }
    }
}
