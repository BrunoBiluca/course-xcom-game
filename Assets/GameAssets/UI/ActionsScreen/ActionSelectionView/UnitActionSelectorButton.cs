using TMPro;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class UnitActionSelectorButton : MonoBehaviour
    {
        [field: SerializeField] public UnitActionsFactory.Actions Action { get; private set; }

        private UnitActionSelectionView selector;

        public void Awake()
        {
            var text = transform.FindComponent<TextMeshProUGUI>("text");
            text.text = Action.ToString();
        }

        public void Setup(UnitActionSelectionView selector)
        {
            this.selector = selector;
        }

        public void Select()
        {
            selector.Select(Action);
        }
    }
}
