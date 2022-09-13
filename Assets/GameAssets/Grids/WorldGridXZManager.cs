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
        private int rangeDistance;

        public WorldGridXZManager(
            IWorldGridXZ<T> worldGrid
        )
        {
            Grid = worldGrid;
        }

        public void SetRangeValidation(Vector3 position, int distance)
        {
            currCell = Grid.GetCell(position);
            rangeDistance = distance;
        }

        public void ResetRangeValidation()
        {
            currCell = null;
            rangeDistance = 0;
        }

        public IEnumerable<GridCellXZ<T>> GetAllAvailableCells()
        {
            foreach(var c in Grid.Cells)
            {
                if(!c.IsEmpty())
                    continue;

                if(currCell != null && !c.IsInRange(currCell, rangeDistance))
                    continue;

                yield return c;
            }
        }

        public bool IsCellAvailable(GridCellXZ<T> cell)
        {
            return currCell != null
                && cell.IsEmpty()
                && cell.IsInRange(currCell, rangeDistance);
        }

        public bool IsCellAvailable(Vector3 position)
        {
            var cell = Grid.GetCell(position);
            return IsCellAvailable(cell);
        }
    }
}