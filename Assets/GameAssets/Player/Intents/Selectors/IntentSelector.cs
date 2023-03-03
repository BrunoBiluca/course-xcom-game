using System;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{

    public sealed class IntentSelector
        : IGridIntentSelector
        , IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IActorSelector<IAPActor> unitActorSelector;

        public event Action<IGridIntent> OnIntentSelected;
        public event Action OnIntentUnselected;

        public Optional<IGridIntent> CurrentIntent { get; private set; }

        public IntentSelector(
            IActorSelector<IAPActor> unitActorSelector
        )
        {
            CurrentIntent = Optional<IGridIntent>.None();
            this.unitActorSelector = unitActorSelector;

            unitActorSelector.OnUnitUnselected += UnselectIntent;
        }

        public void SetIntent(IGridIntent action)
        {
            var currentActor = unitActorSelector.CurrentUnit;
            if(currentActor == null)
                throw new ActorIsNotSelected();

            if(currentActor.ActionPoints.IsEmpty)
                throw new NoAPAvaiable();

            CurrentIntent = Optional<IGridIntent>.Some(action);
            Logger?.Log("Action", action.GetType().ToString(), "was selected");
            OnIntentSelected?.Invoke(action);

            currentActor.OnCantExecuteAction -= CantExecuteActionHandle;
            currentActor.OnCantExecuteAction += CantExecuteActionHandle;

            currentActor.OnActionExecuted -= UnselectIntent;
            currentActor.OnActionExecuted += UnselectIntent;

            currentActor.Set(action);
        }

        private void CantExecuteActionHandle()
        {
            DebugPopup.Create("Can't execute action");
            Logger?.Log("Can't execute", CurrentIntent.Get().GetType().ToString());
            UnselectIntent();
        }

        public void UnselectIntent()
        {
            unitActorSelector.CurrentUnit?.UnsetAction();
            OnIntentUnselected?.Invoke();

            if(!CurrentIntent.IsPresent) return;

            Logger?.Log("Action", CurrentIntent.Get().GetType().ToString(), "was deselected");
            CurrentIntent = Optional<IGridIntent>.None();
        }
    }
}