using System;
using UnityEngine;

namespace GameAssets
{
    public class MoveUnitAction : IUnitAction
    {
        private readonly UnitMono unit;
        private readonly IWorldCursor worldCursor;
        private readonly WorldGridXZManager<GridUnitValue> gridManager;

        public event Action OnFinishAction;
        public event Action OnCantExecuteAction;

        public bool ExecuteImmediatly => false;

        public MoveUnitAction(
            UnitMono unit,
            IWorldCursor worldCursor,
            WorldGridXZManager<GridUnitValue> gridManager
        )
        {
            this.unit = unit;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
        }

        public void Execute()
        {
            if(!CanExecute())
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            // TODO: implementar uma factory de animações
            unit.AnimatorController.Play(new WalkingAnimation(true));

            worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos);

            unit.TransformNav.SetDestination(pos);
            unit.TransformNav.OnReachDestination += FinishNavegation;

            unit.SetUpdateCallback(UpdateNavegation);
        }

        public void UpdateNavegation(float time)
        {
            unit.TransformNav.Update(time);
        }

        public void ApplyValidation()
        {
            gridManager.SetRangeValidation(
                unit.Transform.Position,
                unit.UnitConfigTemplate.MovementRange
            );
        }

        public bool CanExecute()
        {
            if(!worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos))
                return false;

            if(!gridManager.IsCellAvailable(pos))
                return false;

            return true;
        }

        private void FinishNavegation()
        {
            unit.TransformNav.OnReachDestination -= FinishNavegation;
            unit.AnimatorController.Play(new WalkingAnimation(false));

            gridManager.ResetRangeValidation();

            OnFinishAction?.Invoke();
        }
    }
}
