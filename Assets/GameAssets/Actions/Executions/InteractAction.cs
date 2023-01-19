using System;
using UnityFoundation.CharacterSystem.ActorSystem;

namespace GameAssets
{
    public class InteractAction : IAction
    {
        private readonly IInteractableUnit interactable;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public InteractAction(IInteractableUnit unit)
        {
            interactable = unit;
        }

        public void Execute()
        {
            interactable.Interact();
            OnFinishAction?.Invoke();
        }
    }
}
