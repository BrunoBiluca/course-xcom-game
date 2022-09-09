using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class GridXZMono : MonoBehaviour
    {
        public IWorldGridXZ<GridUnitValue> Grid { get; private set; }

        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Depth { get; private set; }
        [field: SerializeField] public int CellSize { get; private set; }

        public void Awake()
        {
            if(Grid == null)
                Setup();
        }

        public void Setup()
        {
            Grid = new WorldGridXZ<GridUnitValue>(
                transform.position,
                Width,
                Depth,
                CellSize
            );
        }
    }
}
