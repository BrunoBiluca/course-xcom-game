using System;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class MoveUnitAction : IAction
    {
        private readonly PlayerUnit unit;
        private readonly IAsyncProcessor asyncProcessor;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridManager gridManager;

        public event Action OnFinishAction;
        public event Action OnCantExecuteAction;

        public bool ExecuteImmediatly => false;

        public MoveUnitAction(
            PlayerUnit unit,
            IAsyncProcessor asyncProcessor,
            IWorldCursor worldCursor,
            UnitWorldGridManager gridManager
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

            // TODO: implementar uma factory de anima??es
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

            OnFinishAction?.Invoke();
        }
    }
}
