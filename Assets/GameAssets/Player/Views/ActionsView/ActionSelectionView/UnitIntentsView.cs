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
        IDependencySetup<ICharacterSelector, IGridIntentSelector, UnitIntentsFactory, ActionsConfig>
    {
        [SerializeField] private GameObject actionSelectorPrefab;
        private UnitActionsEnum? currentAction;

        private IGridIntentSelector intentSelector;
        private ActionsConfig actionsConfig;
        private ICharacterSelector selector;
        private UnitIntentsFactory factory;
        private List<UnitIntentView> buttons = new();

        public IBilucaLogger Logger { get; set; }

        protected override void OnFirstShow()
        {
            InstantiateButtons();
        }

        private void InstantiateButtons()
        {
            foreach(UnitActionsEnum a in Enum.GetValues(typeof(UnitActionsEnum)))
            {
                var selector = Instantiate(actionSelectorPrefab, transform)
                    .GetComponent<UnitIntentView>();

                var desc = $"(AP: {actionsConfig.GetCost(a)}) - {GetActionDescription(a)}";
                selector.Setup(this, a, desc);
                buttons.Add(selector);
            }
        }

        private string GetActionDescription(UnitActionsEnum action)
        {
            return action switch {
                UnitActionsEnum.MOVE => "Move unit to designated place.",
                UnitActionsEnum.SPIN => "Turn around, that is it.",
                UnitActionsEnum.SHOOT => "Shot on enemy unit.",
                UnitActionsEnum.GRENADE => "Throw grenade to designated place.",
                UnitActionsEnum.MELEE => "Attack physically an enemy unit",
                UnitActionsEnum.INTERACT => "Interact with objects",
                UnitActionsEnum.METEOR => "Redirect a meteor to designated place.",
                _ => throw new NotImplementedException(),
            };
        }

        public void Setup(
            ICharacterSelector selector,
            IGridIntentSelector intentSelector,
            UnitIntentsFactory factory,
            ActionsConfig actionsConfig
        )
        {
            this.factory = factory;
            this.intentSelector = intentSelector;
            this.actionsConfig = actionsConfig;

            this.selector = selector;
            selector.OnUnitSelected += Show;
            selector.OnUnitUnselected += Hide;
        }

        protected override void OnShow()
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

        protected override void OnHide() => CleanActions();

        private void CleanActions()
        {
            currentAction = null;
            foreach(var b in buttons)
                b.SetColor(Color.white);
        }
    }
}
