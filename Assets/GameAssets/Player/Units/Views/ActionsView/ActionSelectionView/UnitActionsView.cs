using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using static log4net.Appender.ColoredConsoleAppender;

namespace GameAssets
{
    public class UnitActionsView
        : MonoBehaviour,
        IBilucaLoggable,
        IDependencySetup<ICharacterSelector, IActionSelector<IAPIntent>, UnitIntentsFactory>
    {
        [SerializeField] private GameObject actionSelectorPrefab;
        private UnitActionsEnum? currentAction;

        private IActionSelector<IAPIntent> actionSelector;
        private ICharacterSelector selector;
        private UnitIntentsFactory factory;
        private List<UnitActionSelector> buttons;

        public IBilucaLogger Logger { get; set; }

        public void Awake()
        {
            InstantiateButtons();
        }

        private void InstantiateButtons()
        {
            buttons = new List<UnitActionSelector>();
            foreach(UnitActionsEnum a in Enum.GetValues(typeof(UnitActionsEnum)))
            {
                var selector = Instantiate(actionSelectorPrefab, transform)
                    .GetComponent<UnitActionSelector>();

                selector.Setup(this, a);
                buttons.Add(selector);
            }
        }

        public void Setup(
            ICharacterSelector selector,
            IActionSelector<IAPIntent> actionSelector,
            UnitIntentsFactory factory
        )
        {
            this.factory = factory;
            this.actionSelector = actionSelector;

            this.selector = selector;
            selector.OnUnitSelected += HandleUnitSelected;
            actionSelector.OnActionUnselected += CleanActions;
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
                actionSelector.UnselectAction();
                return;
            }

            try
            {
                var unitIntent = factory.Get(actionType);
                actionSelector.SetIntent(unitIntent);
                unitIntent.GridValidation();
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
