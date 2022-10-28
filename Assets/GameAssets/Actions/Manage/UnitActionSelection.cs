using System;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public sealed class UnitActionHandler : IUnitActionSelector, IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IUnitActorSelector unitActorSelector;

        public event Action<IUnitAction> OnActionSelected;
        public event Action OnActionDeselected;

        public Optional<IUnitAction> CurrentAction { get; private set; }

        public UnitActionHandler(
            IUnitActorSelector unitActorSelector
        )
        {
            CurrentAction = Optional<IUnitAction>.None();
            this.unitActorSelector = unitActorSelector;
        }

        public void SetAction(IUnitAction action)
        {
            if(unitActorSelector.CurrentUnitActor == null)
                throw new ActorIsNotSelected();

            CurrentAction = Optional<IUnitAction>.Some(action);
            OnActionSelected?.Invoke(action);

            unitActorSelector.CurrentUnitActor.OnCantExecuteAction -= CantExecuteActionHandle;
            unitActorSelector.CurrentUnitActor.OnCantExecuteAction += CantExecuteActionHandle;

            action.OnFinishAction -= DeselectAction;
            action.OnFinishAction += DeselectAction;

            unitActorSelector.CurrentUnitActor.SetAction(CurrentAction);

            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was selected");
        }

        public void CantExecuteActionHandle()
        {
            DebugPopup.Create("Can't execute action");
            Logger?.Log("Can't execute", CurrentAction.Get().GetType().ToString());

            DeselectAction();
        }

        public void DeselectAction()
        {
            if(!CurrentAction.IsPresentAndGet(out IUnitAction action)) return;

            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was deselected");
            CurrentAction = Optional<IUnitAction>.None();
            OnActionDeselected?.Invoke();
        }
    }
}