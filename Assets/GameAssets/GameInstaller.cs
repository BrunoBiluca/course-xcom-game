using UnityEngine;
using UnityFoundation.Code;
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

        public PrettyObject BePretty()
        {
            var installerColor = new Color(.38f, .35f, .06f);
            return new PrettyObject(false, installerColor, Color.white, gameObject);
        }

        protected override void OnAwake()
        {
            grid.Setup();

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


            foreach(var unit in FindObjectsOfType<UnitMono>())
            {
                unit.Setup(worldCursor, gridManager);
            }
        }
    }
}
