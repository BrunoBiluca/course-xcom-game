using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class PlayerInputsView
        : BaseView
        , IDependencySetup<ICharacterSelector, IGridIntentSelector>
    {
        private ICharacterSelector selector;
        private IGridIntentSelector intentSelector;

        private GameObject selectedUnitInput;
        private GameObject unselectedUnitInput;
        private GameObject takeActionInput;

        protected override void OnAwake()
        {
            selectedUnitInput = transform.FindTransform("select_unit_input").gameObject;
            unselectedUnitInput = transform.FindTransform("unselect_unit_input").gameObject;
            takeActionInput = transform.FindTransform("take_action_input").gameObject;
        }

        public void Setup(ICharacterSelector selector, IGridIntentSelector intentSelector)
        {
            this.selector = selector;
            this.intentSelector = intentSelector;
        }

        protected override void OnFirstShow()
        {
            Display();
        }

        public void Display()
        {
            selector.OnUnitSelected += HandleUnitSelected;
            selector.OnUnitUnselected += HandleUnitUnselected;

            intentSelector.OnIntentSelected += HandleIntentSelected;
            intentSelector.OnIntentUnselected += HandleIntentUnselected;
        }

        protected override void OnHide()
        {
            if(selector != null)
            {
                selector.OnUnitSelected -= HandleUnitSelected;
                selector.OnUnitUnselected -= HandleUnitUnselected;
            }

            if(intentSelector != null)
            {
                intentSelector.OnIntentSelected -= HandleIntentSelected;
                intentSelector.OnIntentUnselected -= HandleIntentUnselected;
            }
        }

        private void HandleIntentSelected(IGridIntent intent)
        {
            takeActionInput.SetActive(true);
        }
        private void HandleIntentUnselected()
        {
            takeActionInput.SetActive(false);
        }

        private void HandleUnitSelected()
        {
            selectedUnitInput.SetActive(false);
            unselectedUnitInput.SetActive(true);
        }

        private void HandleUnitUnselected()
        {
            selectedUnitInput.SetActive(true);
            unselectedUnitInput.SetActive(false);
        }
    }
}
