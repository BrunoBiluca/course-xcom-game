using System;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class UnitActionSelectionView : MonoBehaviour, IUnitActionSelector
    {
        public event Action<IUnitAction> OnActionSelected;

        private UnitActionsFactory factory;

        public void Awake()
        {
        }

        public void Start()
        {
            var buttons = transform
                .FindComponentsInChildren<UnitActionSelectorButton>("action");
            foreach(var button in buttons)
            {
                button.Setup(this);
            }
        }

        public void Setup(
            UnitSelectionMono unitSelection,
            UnitActionsFactory factory
        )
        {
            this.factory = factory;

            unitSelection.OnUnitSelected += Show;
            unitSelection.OnUnitDeselected += Hide;
        }

        private void Show()
        {
            gameObject.SetActive(true);
            UnityDebug.I.Log(nameof(UnitActionSelectionView), "was shown");
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            UnityDebug.I.Log(nameof(UnitActionSelectionView), "was hidden");
        }

        public void Select(UnitActionsFactory.Actions action)
        {
            OnActionSelected?.Invoke(factory.Get(action));
        }
    }
}
