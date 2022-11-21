using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;

namespace GameAssets
{
    public class APUnitActionSelectionView : MonoBehaviour
    {
        private UnitActionsEnum? currentAction;

        private IUnitActionSelector<IAPUnitAction> actionSelector;
        private IUnitActionsFactory<IAPUnitAction> factory;
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
            IUnitActionSelector<IAPUnitAction> actionSelector,
            IUnitActionsFactory<IAPUnitAction> factory
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
            actionSelector.SetAction(factory.Get(actionType));
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
