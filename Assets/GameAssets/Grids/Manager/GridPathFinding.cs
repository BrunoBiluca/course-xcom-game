using UnityFoundation.Code.Algorithms;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code;
using System.Collections.Generic;

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
        private PathFinding pathFindingGrid;

        public GridPathFinding(IGridXZCells<UnitValue> grid)
        {
            this.grid = grid;
        }

        public GridPath Evaluate(GridCellXZ<UnitValue> start, GridCellXZ<UnitValue> end)
        {
            var path = pathFindingGrid.FindPath(
                new Int2(start.Position.X, start.Position.Z),
                new Int2(end.Position.X, end.Position.Z)
            );

            List<Int2> steps = new();
            foreach(var step in path)
                steps.Add(step);

            return new GridPath(steps.Count - 1, steps.ToArray());
        }

        public void BuildPathFindingGrid()
        {
            var gridSize = new PathFinding.GridSize(grid.Width, grid.Depth);
            pathFindingGrid = new PathFinding(gridSize);

            foreach(var cell in grid.Cells)
            {
                if(IsCellBlocked(cell))
                    pathFindingGrid.AddBlocked(new Int2(cell.Position.X, cell.Position.Z));
            }
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
