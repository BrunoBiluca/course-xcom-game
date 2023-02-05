using UnityEngine;
using UnityFoundation.Code.Algorithms;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code;
using System.Linq;

namespace GameAssets
{
    public class GridPath
    {
        public int Steps { get; private set; }
        public Int2[] Positions { get; private set; }

        public GridPath(int steps, Int2[] positions)
        {
            Steps = steps;
            Positions = positions;
        }
    }

    public class GridPathFinding
    {
        private readonly IGridXZCells<UnitValue> grid;

        public GridPathFinding(IGridXZCells<UnitValue> grid)
        {
            this.grid = grid;
        }

        public GridPath Evaluate(GridCellXZ<UnitValue> start, GridCellXZ<UnitValue> end)
        {
            var pathFinding = BuildPathFindingGrid();

            var path = pathFinding.FindPath(
                new Int2(start.Position.X, start.Position.Z),
                new Int2(end.Position.X, end.Position.Z)
            );

            var steps = path.Count() - 1;
            return new GridPath(steps, path.ToArray());
        }

        private PathFinding BuildPathFindingGrid()
        {
            var gridSize = new PathFinding.GridSize(grid.Width, grid.Depth);
            var pathFinding = new PathFinding(gridSize);

            foreach(var cell in grid.Cells)
            {
                if(IsCellBlocked(cell))
                    pathFinding.AddBlocked(new Int2(cell.Position.X, cell.Position.Z));
            }
            return pathFinding;
        }

        private bool IsCellBlocked(GridCellXZ<UnitValue> cell)
        {
            foreach(var u in cell.Value.Units)
                if(u.IsBlockable)
                    return true;

            return false;
        }
    }
}
