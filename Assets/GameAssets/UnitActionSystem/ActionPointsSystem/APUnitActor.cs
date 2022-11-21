using System;
using UnityFoundation.Code;
using UnityFoundation.ResourceManagement;

namespace GameAssets
{
    public class APUnitActor : IAPUnitActor
    {
        public Optional<IAPUnitAction> CurrentAction { get; private set; }
        public IResourceManager ActionPoints { get; private set; }

        public event Action OnCantExecuteAction;

        public APUnitActor(IResourceManager actionPoints)
        {
            CurrentAction = Optional<IAPUnitAction>.None();
            ActionPoints = actionPoints;
        }

        public void Execute()
        {
            if(!CurrentAction.IsPresentAndGet(out IAPUnitAction action))
                return;

            if(!ActionPoints.TrySubtract((uint)action.ActionPointsCost))
            {
                InvokeCantExecuteAction();
                return;
            }

            action.Execute();
        }

        public void Set(IAPUnitAction action)
        {
            if(action == null)
                throw new ArgumentNullException(
                    "Set action should not be null, use UnsetAction instead."
                );

            CurrentAction = Optional<IAPUnitAction>.Some(action);
            SubscribeActionEvents();

            action.ApplyValidation();
            if(action.ExecuteImmediatly)
                Execute();
        }

        public void UnsetAction()
        {
            UnubscribeActionEvents();
            CurrentAction.Get().ResetValidation();
            CurrentAction = Optional<IAPUnitAction>.None();
        }

        private void SubscribeActionEvents()
        {
            UnubscribeActionEvents();
            CurrentAction.Some(a => a.OnCantExecuteAction += InvokeCantExecuteAction);
        }

        private void UnubscribeActionEvents()
        {
            CurrentAction.Some(a => a.OnCantExecuteAction -= InvokeCantExecuteAction);
        }

        private void InvokeCantExecuteAction()
        {
            OnCantExecuteAction?.Invoke();
        }
    }
}
