using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class GridXZMonoDebug : MonoBehaviour
    {
        [field: SerializeField] public bool DebugMode { get; private set; }

        [SerializeField] private GridXZMono gridMono;
        private IWorldGridXZ<GridDebugValue> grid;

        private IWorldCursor worldCursor;
        [SerializeField] private GameObject worldCursorRef;

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

            if(worldCursor == null)
                return;

            worldCursor.WorldPosition.Some(pos => {
                var gridValue = grid.GetValue((int)pos.x, (int)pos.z);

                if(gridValue.Text == null)
                    return;

                gridValue.Text.color = Color.red;
            });
        }

        public void Display()
        {
            TransformUtils.RemoveChildObjects(transform);

            for(int x = 0; x < grid.GridMatrix.GetLength(0); x++)
            {
                for(int z = 0; z < grid.GridMatrix.GetLength(1); z++)
                {
                    var gridPos = grid.GridMatrix[x, z].GridPosition;
                    var text = DebugDraw.DrawWordTextCell(
                        grid.GridMatrix[x, z].ToString(),
                        grid.GetCellWorldPosition(gridPos),
                        new Vector3(grid.CellSize, 0.5f, grid.CellSize),
                        fontSize: 1f,
                        transform
                    );

                    gridMono.Grid.TrySetValue(x, z, new GridDebugValue(text));

                    Debug.DrawLine(
                        grid.GetCellWorldPosition(gridPos),
                        grid.GetCellWorldPosition(gridPos.TranslateZ(grid.CellSize)),
                        Color.white,
                        100f
                    );
                    Debug.DrawLine(
                        grid.GetCellWorldPosition(gridPos),
                        grid.GetCellWorldPosition(gridPos.TranslateZ(grid.CellSize)),
                        Color.white,
                        100f
                    );
                }
            }
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
