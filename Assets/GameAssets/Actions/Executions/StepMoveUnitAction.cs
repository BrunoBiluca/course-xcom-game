using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Algorithms;
using UnityFoundation.Code.Grid;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class StepMoveUnitAction : IAction
    {
        private readonly IUnit unit;
        private readonly UnitWorldGridXZManager gridManager;
        private readonly IWorldCursor worldCursor;
        private readonly IAsyncProcessor asyncProcessor;
        private List<Int2> path;
        private int step;

        public StepMoveUnitAction(
            IUnit unit,
            UnitWorldGridXZManager gridManager,
            IWorldCursor worldCursor,
            IAsyncProcessor asyncProcessor
        )
        {
            this.unit = unit;
            this.gridManager = gridManager;
            this.worldCursor = worldCursor;
            this.asyncProcessor = asyncProcessor;
        }

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public void Execute()
        {
            unit.AnimatorController.Play(new WalkingAnimation(true));

            worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos);

            var gridSize = new PathFinding.GridSize(
                gridManager.Grid.Width,
                gridManager.Grid.Depth
            );
            var pathFinding = new PathFinding(gridSize);

            gridManager.Validator().WhereIsNotEmpty().Apply();

            foreach(var cell in gridManager.GetAllAvailableCells())
            {
                pathFinding.AddBlocked(new Int2(cell.Position.X, cell.Position.Z));
            }

            gridManager.ResetValidation();

            var unitCell = gridManager.Grid.GetCell(unit.Transform.Position);
            var targetCell = gridManager.Grid.GetCell(pos);

            path = pathFinding.FindPath(
                new Int2(unitCell.Position.X, unitCell.Position.Z),
                new Int2(targetCell.Position.X, targetCell.Position.Z)
            ).ToList();

            if(path.Count == 1)
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            step = 1;
            Move();

            asyncProcessor.ExecuteEveryFrame(UpdateNavegation);
        }

        private void Move()
        {
            if(step == path.Count - 1)
            {
                unit.AnimatorController.Play(new WalkingAnimation(false));
                OnFinishAction?.Invoke();
                return;
            }

            var nextPosition = gridManager.Grid
                .GetCellCenterPosition(new GridCellPositionXZ(path[step].X, path[step].Y));
            unit.TransformNav.SetDestination(nextPosition);

            unit.TransformNav.OnReachDestination -= ReachDestination;
            unit.TransformNav.OnReachDestination += ReachDestination;
        }

        private void ReachDestination()
        {
            step++;
            Move();
        }

        public void UpdateNavegation(float time)
        {
            unit.TransformNav.Update(time);
        }
    }
}
