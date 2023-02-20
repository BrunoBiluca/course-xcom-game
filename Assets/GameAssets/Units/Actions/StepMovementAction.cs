using System;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class StepMovementAction : IAction, IBilucaLoggable
    {
        private readonly ICharacterUnit unit;
        private readonly IUnitWorldGridManager gridManager;
        private readonly Vector3 position;
        private readonly IAsyncProcessor asyncProcessor;
        private GridPath path;
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

            var unitCell = gridManager.Grid.GetCell(unit.Transform.Position);
            var targetCell = gridManager.Grid.GetCell(position);
            path = new GridPathFinding(gridManager.Grid).Evaluate(unitCell, targetCell);

            if(path.Steps == 0)
            {
                Logger?.LogHighlight(unit.Name, "can't move to", position.ToString());
                OnCantExecuteAction?.Invoke();
                return;
            }

            StartMovement();
        }

        private void StartMovement()
        {
            Logger?.LogHighlight(unit.Name, "Start movement with path", string.Join("\n", path));
            step = 0;

            unit.AnimatorController.Play(new WalkingAnimation(true));
            unit.TransformNav.OnReachDestination += ReachDestination;

            Move();
            asyncProcessor.ExecuteEveryFrame(UpdateNavegation);
        }

        private void Move()
        {
            if(step == path.Steps + 1)
            {
                FinishMovement();
                return;
            }

            var nextPosition = gridManager.Grid
                .GetCellCenterPosition(
                    new GridCellPositionXZ(path.Positions[step].X, path.Positions[step].Y)
                );
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
