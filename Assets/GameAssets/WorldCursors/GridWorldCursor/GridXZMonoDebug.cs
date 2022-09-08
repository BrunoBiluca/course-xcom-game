using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class GridXZMonoDebug : MonoBehaviour
    {
        [SerializeField] private GameObject worldCursorRef;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GridXZMono gridMono;
        [field: SerializeField] public bool DebugMode { get; private set; }

        private IWorldGridXZ<GridUnitValue> grid;
        private IWorldCursor worldCursor;

        public void Awake()
        {
            worldCursor = worldCursorRef.GetComponent<IWorldCursor>();
        }

        public void Start()
        {
            grid = gridMono.Grid;
            Display();
        }

        public void Update()
        {
            gameObject.SetActive(DebugMode);
        }

        public void Display()
        {
            TransformUtils.RemoveChildObjects(transform);

            for(int x = 0; x < grid.Cells.GetLength(0); x++)
                for(int z = 0; z < grid.Cells.GetLength(1); z++)
                    DrawGridCell(x, z);

            DrawGridBorders();
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

            var text = DebugDraw.DrawWordTextCell(
                grid.Cells[x, z].ToString(),
                cellWorldPos,
                new Vector3(grid.CellSize, 0.5f, grid.CellSize),
                fontSize: 2f,
                transform
            );

            gridMono.Grid.TrySetValue(text.transform.position, new GridUnitValue(text));

            Debug.DrawLine(
                cellWorldPos,
                cellWorldPos + Vector3.forward * grid.CellSize,
                Color.white,
                100f
            );
            Debug.DrawLine(
                cellWorldPos,
                cellWorldPos + Vector3.right * grid.CellSize,
                Color.white,
                100f
            );
        }

        private void DrawGridBorders()
        {
            Debug.DrawLine(
                grid.DepthPosition,
                grid.WidthAndDepthPosition,
                Color.white,
                100f
            );

            Debug.DrawLine(
                grid.WidthPosition,
                grid.WidthAndDepthPosition,
                Color.white,
                100f
            );
        }
    }
}
