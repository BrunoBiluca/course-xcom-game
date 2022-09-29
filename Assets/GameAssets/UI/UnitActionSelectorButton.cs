using TMPro;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class UnitActionSelectorButton : MonoBehaviour
    {
        [SerializeField] private UnitActionsFactory.Actions action;

        private UnitActionSelectionView selector;

        public void Awake()
        {
            var text = transform.FindComponent<TextMeshProUGUI>("text");
            text.text = action.ToString();
        }

        public void Setup(UnitActionSelectionView selector)
        {
            this.selector = selector;
        }

        public void Select()
        {
            selector.Select(action);
        }
    }
}
