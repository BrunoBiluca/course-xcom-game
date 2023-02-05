using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Algorithms;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using System.Linq;

namespace GameAssets
{
    public class UnitDirectRangeValidation : IGridValidation<UnitValue>
    {
        private readonly IGridXZCells<UnitValue> grid;
        private readonly GridCellXZ<UnitValue> baseCell;
        private readonly int range;

        public UnitDirectRangeValidation(
            IGridXZCells<UnitValue> grid,
            GridCellXZ<UnitValue> baseCell,
            int range
        )
        {
            this.grid = grid;
            this.baseCell = baseCell;
            this.range = range;
        }

        public bool IsAvailable(GridCellXZ<UnitValue> cell)
        {
            if(!cell.IsInRange(baseCell, range))
                return false;

            var path = new GridPathFinding(grid).Evaluate(baseCell, cell);

            return path.Steps > 0 && path.Steps <= range;
        }
    }
}
