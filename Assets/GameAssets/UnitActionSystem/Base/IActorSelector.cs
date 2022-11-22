using System;

namespace GameAssets
{
    public interface IActorSelector<TActor>
    {
        public TActor CurrentUnitActor { get; }

        event Action OnUnitSelected;
        event Action OnUnitUnselected;

        void UnselectUnit();
    }
}