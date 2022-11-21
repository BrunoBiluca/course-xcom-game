using System;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    // TODO: esse cara é importante de ter testes, vai ser utilizado em qualquer sistema que deve se selecionar uma action de um actor
    public sealed class APUnitActionSelection
        : IUnitActionSelector<IAPUnitAction>, IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IUnitActorSelector<IAPUnitActor> unitActorSelector;

        public event Action<IAPUnitAction> OnActionSelected;
        public event Action OnActionUnselected;

        public Optional<IAPUnitAction> CurrentAction { get; private set; }

        public APUnitActionSelection(
            IUnitActorSelector<IAPUnitActor> unitActorSelector
        )
        {
            CurrentAction = Optional<IAPUnitAction>.None();
            this.unitActorSelector = unitActorSelector;

            unitActorSelector.OnUnitUnselected += UnselectAction;
        }

        public void SetAction(IAPUnitAction action)
        {
            if(unitActorSelector.CurrentUnitActor == null)
                throw new ActorIsNotSelected();

            CurrentAction = Optional<IAPUnitAction>.Some(action);
            OnActionSelected?.Invoke(action);

            unitActorSelector.CurrentUnitActor.OnCantExecuteAction -= CantExecuteActionHandle;
            unitActorSelector.CurrentUnitActor.OnCantExecuteAction += CantExecuteActionHandle;

            action.OnFinishAction -= UnselectAction;
            action.OnFinishAction += UnselectAction;

            unitActorSelector.CurrentUnitActor.Set(action);

            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was selected");
        }

        public void CantExecuteActionHandle()
        {
            DebugPopup.Create("Can't execute action");
            Logger?.Log("Can't execute", CurrentAction.Get().GetType().ToString());

            UnselectAction();
        }

        public void UnselectAction()
        {
            if(!CurrentAction.IsPresentAndGet(out IAPUnitAction action)) return;

            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was deselected");
            CurrentAction = Optional<IAPUnitAction>.None();

            unitActorSelector.CurrentUnitActor?.UnsetAction();
            OnActionUnselected?.Invoke();
        }
    }
}