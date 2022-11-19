using System;

namespace GameAssets
{
    public interface IUnitActorSelector<TActor>
    {
        public TActor CurrentUnitActor { get; }

        event Action OnUnitSelected;
        event Action OnUnitDeselected;
    }
}