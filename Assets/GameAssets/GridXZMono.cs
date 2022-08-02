using TMPro;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public struct GridDebugValue
    {
        public readonly TextMeshPro Text { get; }

        public GridDebugValue(TextMeshPro text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return "";
        }
    }

    public class GridXZMono : MonoBehaviour
    {
        public IGridXZ<GridDebugValue> Grid { get; private set; }

        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Depth { get; private set; }
        [field: SerializeField] public int CellSize { get; private set; }

        public void Awake()
        {
            Grid = new GridXZ<GridDebugValue>(Width, Depth, CellSize);
        }

        public Vector3 GetCellCenterPosition(Vector3 pos)
        {
            var cellPos = Grid.GetCellPosition((int)pos.x, (int)pos.z);
            return new Vector3(
                cellPos.X + CellSize / 2f,
                0f,
                cellPos.Z + CellSize / 2f
            );
        }

        public Vector3 GetCellPosition(int x, int z)
        {
            var cellPos = Grid.GetCellPosition(x, z);
            return new Vector3(
                cellPos.X,
                0f,
                cellPos.Z
            );
        }
    }
}
