using System;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class UnitActionSelectionView : MonoBehaviour
    {
        public event Action<IUnitAction> OnActionSelected;

        private UnitActionsFactory factory;
        private IUnitActionSelector actionSelector;
        private UnitActionSelectorButton[] buttons;

        public void Awake()
        {
            Hide();

            buttons = transform.FindComponentsInChildren<UnitActionSelectorButton>();
            foreach(var button in buttons)
            {
                button.Setup(this);
            }
        }

        public void Setup(
            UnitSelectionMono unitSelection,
            IUnitActionSelector actionSelector,
            UnitActionsFactory factory
        )
        {
            this.factory = factory;
            this.actionSelector = actionSelector;

            actionSelector.OnActionDeselected += CleanActions;

            unitSelection.OnUnitSelected += Show;
            unitSelection.OnUnitDeselected += Hide;
        }

        private void Show()
        {
            gameObject.SetActive(true);
            CleanActions();
            UnityDebug.I.LogHighlight(nameof(UnitActionSelectionView), "was shown");
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            UnityDebug.I.LogHighlight(nameof(UnitActionSelectionView), "was hidden");
        }

        public void Select(UnitActionsFactory.Actions actionType)
        {
            SelectAction(actionType);
            actionSelector.SetAction(factory.Get(actionType));
        }

        private void SelectAction(UnitActionsFactory.Actions action)
        {
            foreach(var b in buttons)
            {
                b.GetComponent<Image>().color = b.Action == action ? Color.red : Color.white;
            }
        }

        private void CleanActions()
        {
            foreach(var b in buttons)
            {
                b.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
