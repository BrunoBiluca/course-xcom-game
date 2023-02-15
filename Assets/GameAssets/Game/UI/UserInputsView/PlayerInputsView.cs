using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class PlayerInputsView: BaseView, IDependencySetup<ICharacterSelector>
    {
        private ICharacterSelector selector;

        private GameObject selectedUnitInput;
        private GameObject unselectedUnitInput;

        protected override void OnAwake()
        {
            selectedUnitInput = transform.FindTransform("select_unit_input").gameObject;
            unselectedUnitInput = transform.FindTransform("unselect_unit_input").gameObject;
        }

        public void Setup(ICharacterSelector selector)
        {
            this.selector = selector;
        }

        protected override void OnFirstShow()
        {
            Display();
        }

        public void Display()
        {
            selector.OnUnitSelected += HandleUnitSelected;
            selector.OnUnitUnselected += HandleUnitUnselected;
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
