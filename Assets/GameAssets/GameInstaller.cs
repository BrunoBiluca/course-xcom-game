using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.Editor.Hierarchy;

namespace GameAssets
{
    public class GameInstaller : Singleton<GameInstaller>, IPrettyable
    {
        [SerializeField] private GridWorldCursor worldCursor;
        [SerializeField] private GridXZMono grid;
        [SerializeField] private GridXZMonoDebug gridDebug;
        [SerializeField] private UnitSelectionMono unitSelection;
        [SerializeField] private UnitActionSelectionView unitActionSelectionView;
        [SerializeField] private UnitsManager unitsManager;
        [SerializeField] private LevelSetupConfig levelSetupConfig;
        [SerializeField] private GameObject unitPrefab;

        public PrettyObject BePretty()
        {
            var installerColor = new Color(.38f, .35f, .06f);
            return new PrettyObject(false, installerColor, Color.white, gameObject);
        }

        protected override void OnAwake()
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

            unitActionSelectionView.Setup(unitSelection, unitActionHandler, unitActionsFactory);


            foreach(var unitSetup in levelSetupConfig.Units)
            {
                var unit = Instantiate(unitPrefab).GetComponent<UnitMono>();
                unit.Setup(unitSetup.UnitTemplate, worldCursor, gridManager);

                unit.Transform.Position = gridManager.Grid
                    .GetCellCenterPosition(
                        new GridCellPositionXZ(unitSetup.PositionX, unitSetup.PositionZ)
                    );
            }
        }
    }
}
