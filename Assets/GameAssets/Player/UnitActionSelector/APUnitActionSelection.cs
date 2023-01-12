using System;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    // TODO: esse cara é importante de ter testes, vai ser utilizado em qualquer sistema que deve se selecionar uma action de um actor
    public sealed class APUnitActionSelection
        : IActionSelector<IAPIntent>, IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IActorSelector<IAPActor> unitActorSelector;

        public event Action<IAPIntent> OnActionSelected;
        public event Action OnActionUnselected;

        public Optional<IAPIntent> CurrentAction { get; private set; }

        public APUnitActionSelection(
            IActorSelector<IAPActor> unitActorSelector
        )
        {
            CurrentAction = Optional<IAPIntent>.None();
            this.unitActorSelector = unitActorSelector;

            unitActorSelector.OnUnitUnselected += UnselectAction;
        }

        public void SetAction(IAPIntent action)
        {
            var currentActor = unitActorSelector.CurrentUnitActor;
            if(currentActor == null)
                throw new ActorIsNotSelected();

            if(currentActor.ActionPoints.IsEmpty)
                throw new NoAPAvaiable();

            CurrentAction = Optional<IAPIntent>.Some(action);
            OnActionSelected?.Invoke(action);

            currentActor.OnCantExecuteAction -= CantExecuteActionHandle;
            currentActor.OnCantExecuteAction += CantExecuteActionHandle;

            currentActor.OnActionFinished -= UnselectAction;
            currentActor.OnActionFinished += UnselectAction;

            currentActor.Set(action);

            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was selected");
        }

        private void CantExecuteActionHandle()
        {
            DebugPopup.Create("Can't execute action");
            Logger?.Log("Can't execute", CurrentAction.Get().GetType().ToString());

            UnselectAction();
        }

        public void UnselectAction()
        {
            if(!CurrentAction.IsPresentAndGet(out IAPIntent action)) return;

            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was deselected");
            CurrentAction = Optional<IAPIntent>.None();

            unitActorSelector.CurrentUnitActor?.UnsetAction();
            OnActionUnselected?.Invoke();
        }
    }
}