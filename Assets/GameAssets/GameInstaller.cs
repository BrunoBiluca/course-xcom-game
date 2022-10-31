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

            var gridManager = new WorldGridXZManager<GridUnitValue>(grid.Grid);

            gridDebug.Setup(gridManager);

            var unitSelection = FindObjectOfType<UnitSelectionMono>();
            unitSelection.Setup(worldCursor);

            unitSelection.OnUnitDeselected += () => gridManager.ResetRangeValidation();

            var unitActionsFactory = new UnitActionsFactory(
                unitSelection,
                worldCursor,
                gridManager
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

            unitsManager.Setup(levelSetupConfig, worldCursor, gridManager);

            var turnSystem = new TurnSystem();
            turnSystem.OnTurnEnded += () => {
                foreach(var u in unitsManager.GetAllUnits())
                {
                    u.ActionPoints.FullReffil();
                }
            };

            turnSystemView.Setup(turnSystem);
        }
    }
}
