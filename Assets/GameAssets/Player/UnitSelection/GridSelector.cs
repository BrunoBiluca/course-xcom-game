using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{

    public sealed class GridSelector : ISelector
    {
        private readonly IUnitWorldGridManager gridManager;
        private readonly SimpleSelector selector = new();

        public Optional<ISelectable> CurrentUnit => selector.Selected;

        public GridSelector(IUnitWorldGridManager gridManager)
        {
            this.gridManager = gridManager;
        }

        public Optional<ISelectable> Select(Vector3 position)
        {
            Unselect();

            var units = gridManager.GetUnitsInRange(position, 0);

            if(!units.IsEmpty())
                selector.Select(units[0].Selectable);

            return CurrentUnit;
        }

        public Optional<T> Select<T>(Vector3 screenPosition) where T : ISelectable
        {
            Select(screenPosition);

            if(CurrentUnit.IsPresentAndGet(out ISelectable value))
                return Optional<T>.Some((T)value);

            return Optional<T>.None();
        }

        public void Unselect()
        {
            selector.Unselect();
        }
    }
}