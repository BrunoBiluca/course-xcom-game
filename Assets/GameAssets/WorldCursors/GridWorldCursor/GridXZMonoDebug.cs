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
                var gridValue = grid.GetValue(pos);

                if(gridValue.Text == null)
                    return;

                gridValue.Text.color = Color.red;
            });
        }

        public void Display()
        {
            TransformUtils.RemoveChildObjects(transform);

            // TODO: isso daqui é abrir o Cells é uma gambs violenta, aqui deveria ser possível iterar por todas a células do grid e as próprias célular terem a informação de posição no mundo.
            for(int x = 0; x < grid.Cells.GetLength(0); x++)
            {
                for(int z = 0; z < grid.Cells.GetLength(1); z++)
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

                    gridMono.Grid.TrySetValue(text.transform.position, new GridDebugValue(text));

                    Debug.DrawLine(
                        cellWorldPos,
                        cellWorldPos + Vector3.forward * grid.CellSize,
                        Color.white,
                        100f
                    );
                    Debug.DrawLine(
                        cellWorldPos,
                        cellWorldPos + Vector3.forward * grid.CellSize,
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
