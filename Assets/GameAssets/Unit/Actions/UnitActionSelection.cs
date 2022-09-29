using UnityFoundation.Code;

namespace GameAssets
{

    public sealed class UnitActionSelection
    {
        private Optional<IUnitAction> action;

        public UnitActionSelection(
            IUnitActionSelector actionSelector
        )
        {
            actionSelector.OnActionSelected += InstantiateAction;
            action = Optional<IUnitAction>.None();
        }

        private void InstantiateAction(IUnitAction action)
        {
            if(action == null) this.action = Optional<IUnitAction>.None();

            this.action = Optional<IUnitAction>.Some(action);

            UnityDebug.I.Log("Action", this.action.GetType().ToString(), "was selected");
        }

        public Optional<IUnitAction> GetAction()
        {
            return action;
        }
    }
}
