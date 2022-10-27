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
            action.OnFinishAction += DeselectAction;

            unitActorSelector.CurrentUnitActor.SetAction(CurrentAction);


            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was selected");
        }

        public void DeselectAction()
        {
            if(!CurrentAction.IsPresentAndGet(out IUnitAction action)) return;

            action.OnFinishAction -= DeselectAction;

            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was deselected");
            CurrentAction = Optional<IUnitAction>.None();
            OnActionDeselected?.Invoke();
        }
    }
}