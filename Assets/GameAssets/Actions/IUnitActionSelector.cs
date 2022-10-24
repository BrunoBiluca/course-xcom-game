using System;
using UnityFoundation.Code;

namespace GameAssets
{
    public interface IUnitActionSelector
    {
        public event Action<IUnitAction> OnActionSelected;
        public event Action OnActionDeselected;

        public Optional<IUnitAction> CurrentAction { get; }

        public void SetAction(IUnitAction action);
    }
}
