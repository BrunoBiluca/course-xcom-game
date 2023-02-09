using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class PlayerInputsView : MonoBehaviour, IDependencySetup<ICharacterSelector>
    {
        private ICharacterSelector selector;

        private GameObject selectedUnitInput;
        private GameObject unselectedUnitInput;

        public void Awake()
        {
            selectedUnitInput = transform.FindTransform("select_unit_input").gameObject;
            unselectedUnitInput = transform.FindTransform("unselect_unit_input").gameObject;
        }

        public void Setup(ICharacterSelector selector)
        {
            this.selector = selector;
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
