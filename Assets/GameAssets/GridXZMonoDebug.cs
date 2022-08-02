using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class GridXZMonoDebug : MonoBehaviour
    {
        [field: SerializeField] public bool DebugMode { get; private set; }

        [SerializeField] private GridXZMono gridMono;

        private IWorldCursor worldCursor;
        [SerializeField] private GameObject worldCursorRef;

        public void Awake()
        {
            worldCursor = worldCursorRef.GetComponent<IWorldCursor>();
        }

        public void Start()
        {
            Display();
        }

        public void Update()
        {
            gameObject.SetActive(DebugMode);

            if(worldCursor == null)
                return;

            worldCursor.WorldPosition.Some(pos => {
                var gridValue = gridMono.Grid.GetValue((int)pos.x, (int)pos.z);

                if(gridValue.Text == null)
                    return;

                gridValue.Text.color = Color.red;
            });
        }

        public void Display()
        {
            TransformUtils.RemoveChildObjects(transform);

            for(int x = 0; x < gridMono.Grid.GridMatrix.GetLength(0); x++)
            {
                for(int z = 0; z < gridMono.Grid.GridMatrix.GetLength(1); z++)
                {
                    var text = DebugDraw.DrawWordTextCell(
                        gridMono.Grid.GridMatrix[x, z].ToString(),
                        gridMono.GetCellPosition(x, z),
                        new Vector3(gridMono.Grid.CellSize, 0.5f, gridMono.Grid.CellSize),
                        fontSize: 1f,
                        transform
                    );

                    gridMono.Grid.TrySetValue(x, z, new GridDebugValue(text));

                    Debug.DrawLine(
                        gridMono.GetCellPosition(x, z),
                        gridMono.GetCellPosition(x, z + 1),
                        Color.white,
                        100f
                    );
                    Debug.DrawLine(
                        gridMono.GetCellPosition(x, z),
                        gridMono.GetCellPosition(x + 1, z),
                        Color.white,
                        100f
                    );
                }
            }
            Debug.DrawLine(
                gridMono.GetCellPosition(0, gridMono.Grid.Depth),
                gridMono.GetCellPosition(gridMono.Grid.Width, gridMono.Grid.Depth),
                Color.white,
                100f
            );
            Debug.DrawLine(
                gridMono.GetCellPosition(gridMono.Grid.Width, 0),
                gridMono.GetCellPosition(gridMono.Grid.Width, gridMono.Grid.Depth),
                Color.white,
                100f
            );
        }
    }
}
