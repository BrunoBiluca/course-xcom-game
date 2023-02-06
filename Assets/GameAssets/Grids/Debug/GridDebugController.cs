using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class GridDebugController : WorldGridDebug<UnitValue>
    {
        [SerializeField] private UnitWorldGridXZ grid;

        public void Start()
        {
            Setup(grid.Grid);
        }
    }
}
