using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;

namespace GameAssets
{
    public class APUnitActionSelectionView : MonoBehaviour
    {
        private UnitActionsEnum? currentAction;

        private IActionSelector<IAPActionIntent> actionSelector;
        private UnitActionsFactory factory;
        private UnitActionSelectorButton[] buttons;

        public void Awake()
        {
            buttons = transform.FindComponentsInChildren<UnitActionSelectorButton>();
            foreach(var button in buttons)
            {
                button.Setup(this);
            }
        }

        public void Setup(
            IActionSelector<IAPActionIntent> actionSelector,
            UnitActionsFactory factory
        )
        {
            this.factory = factory;
            this.actionSelector = actionSelector;

            actionSelector.OnActionUnselected += CleanActions;
        }

        public void Select(UnitActionsEnum actionType)
        {
            if(currentAction == actionType)
            {
                CleanActions();
                actionSelector.UnselectAction();
                return;
            }

            SelectAction(actionType);

            var unitAction = factory.Get(actionType);
            unitAction.ApplyValidation();
            actionSelector.SetAction(unitAction.ActionFactory);
        }

        private void SelectAction(UnitActionsEnum actionType)
        {
            currentAction = actionType;
            foreach(var b in buttons)
            {
                b.GetComponent<Image>().color = b.Action == currentAction ? Color.red : Color.white;
            }
        }

        private void CleanActions()
        {
            currentAction = null;
            foreach(var b in buttons)
            {
                b.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
