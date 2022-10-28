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
                Setup(new GridXZConfig() {
                    Width = Width,
                    Depth = Depth,
                    CellSize = CellSize
                });
        }

        public void Setup(GridXZConfig config)
        {
            Grid = new WorldGridXZ<GridUnitValue>(
                transform.position,
                config.Width,
                config.Depth,
                config.CellSize,
                () => new GridUnitValue()
            );
        }
    }
}
