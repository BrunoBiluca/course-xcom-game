using System;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    // TODO: esse cara ? importante de ter testes, vai ser utilizado em qualquer sistema que deve se selecionar uma action de um actor
    public sealed class ActionSelector
        : IActionSelector<IAPIntent>, IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IActorSelector<IAPActor> unitActorSelector;

        public event Action<IAPIntent> OnActionSelected;
        public event Action OnActionUnselected;

        public Optional<IAPIntent> CurrentAction { get; private set; }

        public ActionSelector(
            IActorSelector<IAPActor> unitActorSelector
        )
        {
            CurrentAction = Optional<IAPIntent>.None();
            this.unitActorSelector = unitActorSelector;

            unitActorSelector.OnUnitUnselected += UnselectAction;
        }

        public void SetIntent(IAPIntent action)
        {
            var currentActor = unitActorSelector.CurrentUnit;
            if(currentActor == null)
                throw new ActorIsNotSelected();

            if(currentActor.ActionPoints.IsEmpty)
                throw new NoAPAvaiable();

            CurrentAction = Optional<IAPIntent>.Some(action);
            Logger?.Log("Action", action.GetType().ToString(), "was selected");
            OnActionSelected?.Invoke(action);

            currentActor.OnCantExecuteAction -= CantExecuteActionHandle;
            currentActor.OnCantExecuteAction += CantExecuteActionHandle;

            currentActor.OnActionFinished -= UnselectAction;
            currentActor.OnActionFinished += UnselectAction;

            currentActor.Set(action);
        }

        private void CantExecuteActionHandle()
        {
            DebugPopup.Create("Can't execute action");
            Logger?.Log("Can't execute", CurrentAction.Get().GetType().ToString());

            UnselectAction();
        }

        public void UnselectAction()
        {
            unitActorSelector.CurrentUnit?.UnsetAction();
            OnActionUnselected?.Invoke();

            if(!CurrentAction.IsPresentAndGet(out IAPIntent action)) return;

            Logger?.Log("Action", CurrentAction.Get().GetType().ToString(), "was deselected");
            CurrentAction = Optional<IAPIntent>.None();
        }
    }
}