using System;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.ResourceManagement;

namespace GameAssets.ActorSystem
{
    public class APActor : IAPActor
    {
        public Optional<IAPActionIntent> Intent { get; private set; }
        public IResourceManager ActionPoints { get; private set; }

        public event Action OnCantExecuteAction;
        public event Action OnActionFinished;

        private IAction currentAction;

        public APActor(IResourceManager actionPoints)
        {
            Intent = Optional<IAPActionIntent>.None();
            ActionPoints = actionPoints;
        }

        public void Execute()
        {
            if(!Intent.IsPresentAndGet(out IAPActionIntent intent))
                return;

            if(ActionPoints.CurrentAmount < (uint)intent.ActionPointsCost)
            {
                InvokeCantExecuteAction();
                return;
            }

            var action = intent.Create();

            action.OnCantExecuteAction -= InvokeCantExecuteAction;
            action.OnCantExecuteAction += InvokeCantExecuteAction;

            action.OnFinishAction -= InvokeFinishAction;
            action.OnFinishAction += InvokeFinishAction;

            currentAction = action;
            action.Execute();
        }

        private void InvokeCantExecuteAction()
        {
            OnCantExecuteAction?.Invoke();
        }

        public void InvokeFinishAction()
        {
            UnityDebug.I.Log(nameof(currentAction.OnFinishAction));
            ActionPoints.TrySubtract((uint)Intent.Get().ActionPointsCost);
            OnActionFinished?.Invoke();
        }

        public void Set(IAPActionIntent intent)
        {
            if(intent == null)
                throw new ArgumentNullException(
                    "Set action factory should not be null, use UnsetAction instead."
                );

            Intent = Optional<IAPActionIntent>.Some(intent);
            if(intent.ExecuteImmediatly)
                Execute();
        }

        public void UnsetAction()
        {
            Intent = Optional<IAPActionIntent>.None();
        }
    }
}
