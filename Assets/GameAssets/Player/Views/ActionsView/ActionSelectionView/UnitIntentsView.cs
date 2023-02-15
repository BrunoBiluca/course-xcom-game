using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class UnitIntentsView
        : BaseView,
        IBilucaLoggable,
        IDependencySetup<ICharacterSelector, IGridIntentSelector, UnitIntentsFactory>
    {
        [SerializeField] private GameObject actionSelectorPrefab;
        private UnitActionsEnum? currentAction;

        private IGridIntentSelector intentSelector;
        private ICharacterSelector selector;
        private UnitIntentsFactory factory;
        private List<UnitIntentView> buttons;

        public IBilucaLogger Logger { get; set; }

        protected override void OnAwake()
        {
            InstantiateButtons();
        }

        private void InstantiateButtons()
        {
            buttons = new List<UnitIntentView>();
            foreach(UnitActionsEnum a in Enum.GetValues(typeof(UnitActionsEnum)))
            {
                var selector = Instantiate(actionSelectorPrefab, transform)
                    .GetComponent<UnitIntentView>();

                selector.Setup(this, a);
                buttons.Add(selector);
            }
        }

        public void Setup(
            ICharacterSelector selector,
            IGridIntentSelector intentSelector,
            UnitIntentsFactory factory
        )
        {
            this.factory = factory;
            this.intentSelector = intentSelector;

            this.selector = selector;
            selector.OnUnitSelected += HandleUnitSelected;
            intentSelector.OnIntentUnselected += CleanActions;
        }

        private void HandleUnitSelected()
        {
            foreach(var b in buttons)
            {
                b.gameObject.SetActive(selector.CurrentUnit.UnitConfig.Actions.Contains(b.Action));
            }
        }

        public void Select(UnitActionsEnum actionType)
        {
            if(currentAction == actionType)
            {
                CleanActions();
                intentSelector.UnselectIntent();
                return;
            }

            try
            {
                var unitIntent = factory.Get(actionType);
                intentSelector.SetIntent(unitIntent);
                SelectAction(actionType);
            }
            catch(InvalidOperationException ex)
            {
                Logger?.Log($"Unit can't execute action because: {ex.Message}");
                CleanActions();
                intentSelector.UnselectIntent();
            }
        }

        private void SelectAction(UnitActionsEnum actionType)
        {
            currentAction = actionType;
            foreach(var b in buttons)
            {
                b.SetColorIfActive(actionType, Color.red, Color.white);
            }
        }

        private void CleanActions()
        {
            currentAction = null;
            foreach(var b in buttons)
                b.SetColor(Color.white);
        }
    }
}
