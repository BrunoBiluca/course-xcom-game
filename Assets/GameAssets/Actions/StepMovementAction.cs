using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.Algorithms;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class StepMovementAction : IAction, IBilucaLoggable
    {
        private readonly ICharacterUnit unit;
        private readonly IUnitWorldGridManager gridManager;
        private readonly Vector3 position;
        private readonly IAsyncProcessor asyncProcessor;
        private List<Int2> path;
        private int step;

        public IBilucaLogger Logger { get; set; }

        public StepMovementAction(
            IUnitWorldGridManager gridManager,
            IAsyncProcessor asyncProcessor,
            ICharacterUnit unit,
            Vector3 position
        )
        {
            this.unit = unit;
            this.gridManager = gridManager;
            this.position = position;
            this.asyncProcessor = asyncProcessor;
        }

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public void Execute()
        {
            Logger?.LogHighlight(unit.Name, "is moving towards", position.ToString());
            EvaluatePath();

            if(CanMoveToDestination())
            {
                Logger?.LogHighlight(unit.Name, "can't move to", position.ToString());
                OnCantExecuteAction?.Invoke();
                return;
            }

            StartMovement();
        }
        private void EvaluatePath()
        {
            var pathFinding = BuildPathFindingGrid();

            var unitCell = gridManager.Grid.GetCell(unit.Transform.Position);

            var targetCell = gridManager.Grid.GetCell(position);

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
            Logger?.LogHighlight(unit.Name, "Start movement with path", string.Join("\n", path));
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
            Logger?.LogHighlight(unit.Name, "finished moving");
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
