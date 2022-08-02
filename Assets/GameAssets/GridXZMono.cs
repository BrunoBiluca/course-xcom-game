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
        public IWorldGridXZ<GridDebugValue> Grid { get; private set; }

        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Depth { get; private set; }
        [field: SerializeField] public int CellSize { get; private set; }

        public void Awake()
        {
            Grid = new WorldGridXZ<GridDebugValue>(
                transform.position,
                Width,
                Depth,
                CellSize
            );
        }
    }
}
