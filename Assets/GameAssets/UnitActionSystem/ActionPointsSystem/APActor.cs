using System;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.ResourceManagement;

namespace GameAssets
{
    public class APActor : IAPActor
    {
        public Optional<IAPActionIntent> Intent { get; private set; }
        public IResourceManager ActionPoints { get; private set; }

        public event Action OnCantExecuteAction;
        public event Action OnActionFinished;

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
                OnCantExecuteAction?.Invoke();
                return;
            }

            var action = intent.Create();

            action.OnCantExecuteAction += () => OnCantExecuteAction?.Invoke();
            action.OnFinishAction += () => {
                UnityDebug.I.Log(nameof(action.OnFinishAction));
                ActionPoints.TrySubtract((uint)intent.ActionPointsCost);
                OnActionFinished?.Invoke();
            };

            action.Execute();
        }

        public void Set(IAPActionIntent actionFactory)
        {
            if(actionFactory == null)
                throw new ArgumentNullException(
                    "Set action factory should not be null, use UnsetAction instead."
                );

            Intent = Optional<IAPActionIntent>.Some(actionFactory);
            if(actionFactory.ExecuteImmediatly)
                Execute();
        }

        public void UnsetAction()
        {
            Intent = Optional<IAPActionIntent>.None();
        }
    }
}
