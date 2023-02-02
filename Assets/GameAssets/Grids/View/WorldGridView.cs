using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class WorldGridView
        : BilucaMono
        , IDependencySetup<IUnitWorldGridManager, IWorldCursor>
    {
        [SerializeField] private GameObject cellPrefab;

        private IUnitWorldGridManager gridManager;
        private IWorldCursor worldCursor;
        private WorldGridXZ<GridViewValue> grid;

        public void Setup(IUnitWorldGridManager gridManager, IWorldCursor worldCursor)
        {
            this.gridManager = gridManager;
            this.worldCursor = worldCursor;
        }

        public void Display()
        {
            transform.position = gridManager.Grid.InitialPosition;
            grid = new WorldGridXZ<GridViewValue>(
                gridManager.Grid.InitialPosition,
                gridManager.Grid.Width,
                gridManager.Grid.Depth,
                gridManager.Grid.CellSize
            );
            CreateWorldView();
        }

        public void Update()
        {
            if(grid == null) return;
            UpdateCells();
        }

        private void UpdateCells()
        {
            foreach(var c in grid.Cells)
            {
                c.Value.DisableCellRef();
            }

            foreach(var c in gridManager.GetAllAvailableCells())
            {
                var gridValue = grid.Cells[c.Position.X, c.Position.Z].Value;
                gridValue.EnableCellRef();

                switch(gridManager.State)
                {
                    case UnitWorldGridManager.GridState.None:
                        gridValue.SetColor(Color.white);
                        break;
                    case UnitWorldGridManager.GridState.Attack:
                        gridValue.SetColor(Color.red);
                        break;
                    case UnitWorldGridManager.GridState.Interact:
                        gridValue.SetColor(Color.blue);
                        break;
                }
            }

            worldCursor.WorldPosition.Some(pos => {
                grid.TryUpdateValue(pos, view => view.SetColor(Color.green));
            });
        }

        public void CreateWorldView()
        {
            TransformUtils.RemoveChildObjects(transform);

            for(int x = 0; x < grid.Cells.GetLength(0); x++)
                for(int z = 0; z < grid.Cells.GetLength(1); z++)
                    DrawGridCell(x, z);
        }


        private void DrawGridCell(int x, int z)
        {
            var gridCellWorldPos = new Vector3(x * grid.CellSize, 0f, z * grid.CellSize);
            var cellWorldPos = grid.GetCellWorldPosition(gridCellWorldPos);
            var cellWorldCenter = grid.GetCellCenterPosition(gridCellWorldPos);

            var cellRef = Instantiate(cellPrefab, transform);
            cellRef.transform.localPosition = new Vector3(
                cellWorldCenter.x, 0.05f, cellWorldCenter.z
            );
            cellRef.transform.localScale = Vector3.one * grid.CellSize;

            grid.TrySetValue(cellWorldPos, new GridViewValue(cellRef));
        }
    }
}
