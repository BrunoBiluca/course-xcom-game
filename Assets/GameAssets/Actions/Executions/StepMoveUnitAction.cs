using GameAssets.ActorSystem;
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
        private readonly ICharacterUnit unit;
        private readonly UnitWorldGridManager gridManager;
        private readonly IWorldCursor worldCursor;
        private readonly IAsyncProcessor asyncProcessor;
        private List<Int2> path;
        private int step;

        public StepMoveUnitAction(
            ICharacterUnit unit,
            UnitWorldGridManager gridManager,
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
            EvaluatePath();

            if(CanMoveToDestination())
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            StartMovement();
        }
        private void EvaluatePath()
        {
            var pathFinding = BuildPathFindingGrid();

            var unitCell = gridManager.Grid.GetCell(unit.Transform.Position);

            worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos);
            var targetCell = gridManager.Grid.GetCell(pos);

            path = pathFinding.FindPath(
                new Int2(unitCell.Position.X, unitCell.Position.Z),
                new Int2(targetCell.Position.X, targetCell.Position.Z)
            ).ToList();
        }

        private bool CanMoveToDestination()
        {
            return path.Count == 1;
        }

        private PathFinding BuildPathFindingGrid()
        {
            var gridSize = new PathFinding.GridSize(
                gridManager.Grid.Width,
                gridManager.Grid.Depth
            );
            var pathFinding = new PathFinding(gridSize);

            gridManager.Validator().WhereIsNotEmpty().Apply(UnitWorldGridManager.GridState.None);

            foreach(var cell in gridManager.GetAllAvailableCells())
            {
                pathFinding.AddBlocked(new Int2(cell.Position.X, cell.Position.Z));
            }

            gridManager.ResetValidation();
            return pathFinding;
        }

        private void StartMovement()
        {
            step = 1;

            unit.AnimatorController.Play(new WalkingAnimation(true));
            unit.TransformNav.OnReachDestination += ReachDestination;

            Move();
            asyncProcessor.ExecuteEveryFrame(UpdateNavegation);
        }

        private void Move()
        {
            if(step == path.Count)
            {
                FinishMovement();
                return;
            }

            var nextPosition = gridManager.Grid
                .GetCellCenterPosition(new GridCellPositionXZ(path[step].X, path[step].Y));
            unit.TransformNav.SetDestination(nextPosition);
        }

        private void FinishMovement()
        {
            unit.AnimatorController.Play(new WalkingAnimation(false));
            unit.TransformNav.OnReachDestination -= ReachDestination;
            OnFinishAction?.Invoke();
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
