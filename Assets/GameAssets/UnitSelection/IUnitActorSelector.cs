using System;

namespace GameAssets
{
    public interface IUnitActorSelector
    {
        public IUnitActor CurrentUnitActor { get; }

        event Action OnUnitSelected;
        event Action OnUnitDeselected;
    }
}