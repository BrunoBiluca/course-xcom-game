using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class WorldGridView
        : BaseView
        , IDependencySetup<IGridIntentQuery, IUnitWorldGridManager, IWorldCursor>
    {
        [SerializeField] private GameObject cellPrefab;
        private IGridIntentQuery gridIntentQuery;
        private IUnitWorldGridManager gridManager;
        private IWorldCursor worldCursor;
        private WorldGridXZ<GridViewValue> grid;
        private IGridIntent currentIntent;
        private List<GridCellXZ<UnitValue>> cache;

        public void Setup(
            IGridIntentQuery gridIntentQuery,
            IUnitWorldGridManager gridManager,
            IWorldCursor worldCursor
        )
        {
            this.gridIntentQuery = gridIntentQuery;
            this.gridManager = gridManager;
            this.worldCursor = worldCursor;
        }

        protected override void OnFirstShow()
        {
            Display();
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

            DisableAllCells();

            if(gridIntentQuery.CurrentIntent.IsPresentAndGet(out IGridIntent intent))
                UpdateAvaiableCells(intent);

            if(worldCursor.WorldPosition.IsPresentAndGet(out Vector3 position))
                UpdateCursorCell(position);
        }

        private void UpdateCursorCell(Vector3 position)
        {
            if(gridIntentQuery.CurrentIntent.IsPresentAndGet(out IGridIntent intent))
            {
                if(gridIntentQuery.IsCellAvailable(position))
                {
                    UpdateAffectedCells(position, intent);
                }
                return;
            }

            grid.TryUpdateValue(position, view => {
                view.EnableCellRef();
                view.SetColor(Color.green);
            });
        }

        private void UpdateAffectedCells(Vector3 position, IGridIntent intent)
        {
            foreach(var c in gridIntentQuery.GetAffectedCells(position))
            {
                var gridValue = grid.Cells[c.Position.X, c.Position.Z].Value;
                gridValue.EnableCellRef();
                gridValue.SetColor(GridIntentColor.Affected(intent));
            }
        }

        private void UpdateAvaiableCells(IGridIntent intent)
        {
            if(intent != currentIntent)
            {
                currentIntent = intent;
                cache = gridIntentQuery.GetAvaiableCells();
            }

            foreach(var c in cache)
            {
                var gridValue = grid.Cells[c.Position.X, c.Position.Z].Value;
                gridValue.EnableCellRef();
                gridValue.SetColor(GridIntentColor.Avaiable(intent));
            }
        }

        private void DisableAllCells()
        {
            foreach(var c in grid.Cells)
            {
                c.Value.DisableCellRef();
            }
        }

        private void CreateWorldView()
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
