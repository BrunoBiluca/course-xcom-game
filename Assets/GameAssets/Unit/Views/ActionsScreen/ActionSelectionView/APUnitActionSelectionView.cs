using System;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class APUnitActionSelectionView : MonoBehaviour, IBilucaLoggable
    {
        private UnitActionsEnum? currentAction;

        private IActionSelector<IAPIntent> actionSelector;
        private UnitActionsFactory factory;
        private UnitActionSelectorButton[] buttons;

        public IBilucaLogger Logger { get; set; }

        public void Awake()
        {
            buttons = transform.FindComponentsInChildren<UnitActionSelectorButton>();
            foreach(var button in buttons)
            {
                button.Setup(this);
            }
        }

        public void Setup(
            IActionSelector<IAPIntent> actionSelector,
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

            try
            {
                var unitAction = factory.Get(actionType);
                actionSelector.SetAction(unitAction.Intent);
                unitAction.ApplyValidation();
                SelectAction(actionType);
            }
            catch(InvalidOperationException ex)
            {
                Logger?.Log($"Unit can't execute action because: {ex.Message}");
                CleanActions();
                actionSelector.UnselectAction();
            }
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
