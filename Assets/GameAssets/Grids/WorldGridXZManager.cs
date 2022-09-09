using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{

    public class WorldGridXZManager<T>
    {
        public IWorldGridXZ<T> Grid { get; private set; }
        private GridCellXZ<T> currCell;

        public WorldGridXZManager(IWorldGridXZ<T> worldGrid)
        {
            Grid = worldGrid;
        }

        public void UpdateCurrentGridCell(Vector3 position)
        {
            currCell = Grid.GetCell(position);
        }
        public void ResetCurrentGridCell()
        {
            currCell = null;
        }

        public IEnumerable<GridCellXZ<T>> GetAllValidCells()
        {
            foreach(var c in Grid.Cells)
            {
                if(!c.IsEmpty())
                    continue;

                if(currCell != null && !c.IsInRange(currCell, 5))
                    continue;

                yield return c;
            }
        }
    }
}
