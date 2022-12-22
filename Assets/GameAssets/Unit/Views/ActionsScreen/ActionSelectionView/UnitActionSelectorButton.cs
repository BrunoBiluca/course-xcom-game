using TMPro;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class UnitActionSelectorButton : MonoBehaviour
    {
        [field: SerializeField] public UnitActionsEnum Action { get; private set; }

        private APUnitActionSelectionView selector;

        public void Awake()
        {
            var text = transform.FindComponent<TextMeshProUGUI>("text");
            text.text = Action.ToString();
        }

        public void Setup(APUnitActionSelectionView selector)
        {
            this.selector = selector;
        }

        public void Select()
        {
            selector.Select(Action);
        }
    }
}
