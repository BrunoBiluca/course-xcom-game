using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class GridXZMonoDebug : MonoBehaviour
    {
        [field: SerializeField] public bool DebugMode { get; private set; }

        private IWorldGridXZ<GridDebugValue> grid;
        private UnitWorldGridManager gridManager;

        public void Start()
        {
            GameInstaller.I.OnInstallerFinish += () => Setup(GameInstaller.I.GridManager);
        }

        public void Setup(UnitWorldGridManager gridManager)
        {
            grid = new WorldGridXZ<GridDebugValue>(
                gridManager.Grid.InitialPosition,
                gridManager.Grid.Width,
                gridManager.Grid.Depth,
                gridManager.Grid.CellSize
            );
            this.gridManager = gridManager;
            Display();
        }

        public void Update()
        {
            gameObject.SetActive(DebugMode);

            UpdateCells();
        }

        private void UpdateCells()
        {
            if(grid == null) return;

            foreach(var c in grid.Cells)
            {
                c.Value.SetText(gridManager.Grid.Cells[c.Position.X, c.Position.Z].ToString());
            }
        }

        public void Display()
        {
            TransformUtils.RemoveChildObjects(transform);

            for(int x = 0; x < grid.Cells.GetLength(0); x++)
                for(int z = 0; z < grid.Cells.GetLength(1); z++)
                    DrawGridCell(x, z);

            GridDebug.DrawLines(grid, Time.deltaTime);
        }

        private void DrawGridCell(int x, int z)
        {
            var gridCellWorldPos = new Vector3(x * grid.CellSize, 0f, z * grid.CellSize);
            var cellWorldPos = grid.GetCellWorldPosition(gridCellWorldPos);

            var text = DebugDraw.DrawWordTextCell(
                grid.Cells[x, z].ToString(),
                cellWorldPos,
                new Vector3(grid.CellSize, 0.5f, grid.CellSize),
                fontSize: 2f,
                transform
            );

            grid.TrySetValue(cellWorldPos, new GridDebugValue(text));
        }
    }
}
