using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class MoveUnitAction : IUnitAction
    {
        private readonly TrooperUnit unit;
        private readonly IAsyncProcessor asyncProcessor;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridXZManager gridManager;

        public event Action OnFinishAction;
        public event Action OnCantExecuteAction;

        public bool ExecuteImmediatly => false;

        public MoveUnitAction(
            TrooperUnit unit,
            IAsyncProcessor asyncProcessor,
            IWorldCursor worldCursor,
            UnitWorldGridXZManager gridManager
        )
        {
            this.unit = unit;
            this.asyncProcessor = asyncProcessor;
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

            // TODO: implementar uma factory de anima��es
            unit.AnimatorController.Play(new WalkingAnimation(true));

            worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos);

            unit.TransformNav.SetDestination(pos);
            unit.TransformNav.OnReachDestination += FinishNavegation;

            asyncProcessor.ExecuteEveryFrame(UpdateNavegation);
        }

        public void UpdateNavegation(float time)
        {
            unit.TransformNav.Update(time);
        }

        public void ApplyValidation()
        {
            gridManager.Validator()
                .WhereIsEmpty()
                .WithRange(unit.transform.position, unit.UnitConfigTemplate.MovementRange)
                .Apply();
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
            asyncProcessor.ResetCallbackEveryFrame();
            unit.TransformNav.OnReachDestination -= FinishNavegation;
            unit.AnimatorController.Play(new WalkingAnimation(false));

            gridManager.ResetValidation();

            OnFinishAction?.Invoke();
        }

        public void ResetValidation()
        {
            gridManager.ResetValidation();
        }
    }
}
