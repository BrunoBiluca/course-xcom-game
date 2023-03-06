using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public sealed class UnitSelectionMono
        : MonoBehaviour,
        ICharacterSelector,
        IDependencySetup<IWorldCursor, ISelector>,
        IBilucaLoggable
    {
        private IWorldCursor worldCursor;
        private ISelector selector;

        public ICharacterUnit CurrentUnit { get; private set; }
        public IBilucaLogger Logger { get; set; }

        public event Action OnUnitSelected;
        public event Action OnUnitUnselected;

        public void Setup(IWorldCursor worldCursor, ISelector selector)
        {
            this.worldCursor = worldCursor;
            this.selector = selector;
        }

        public void Enable()
        {
            worldCursor.OnClick += TrySelectUnit;
        }

        public void Disable()
        {
            worldCursor.OnClick -= TrySelectUnit;
        }

        private void TrySelectUnit()
        {
            if(!worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos))
            {
                UnselectUnit();
                return;
            }

            UnselectUnit();

            selector.Select<SelectableObject>(pos)
                .Some(SelectUnit)
                .OrElse(UnselectUnit);
        }

        private void SelectUnit(SelectableObject obj)
        {
            var unit = obj.SelectedReference as ICharacterUnit;

            if(CurrentUnit == unit)
                return;

            Logger?.Log("Unit", unit.Name, "was selected");

            CurrentUnit = unit;
            OnUnitSelected?.Invoke();
        }

        public void UnselectUnit()
        {
            if(CurrentUnit != null)
            {
                Logger?.Log("Unit was deselected");
                selector.Unselect();
                CurrentUnit = null;
            }
            OnUnitUnselected?.Invoke();
        }
    }
}