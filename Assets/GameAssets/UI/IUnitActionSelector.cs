using System;

namespace GameAssets
{
    public interface IUnitActionSelector
    {
        public event Action<IUnitAction> OnActionSelected;
    }
}
