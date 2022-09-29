using System;

namespace GameAssets
{
    public sealed class UnitActionsFactory
    {
        private readonly UnitSelectionMono unitSelection;

        public enum Actions
        {
            MOVE,
            SPIN
        }

        public UnitActionsFactory(UnitSelectionMono unitSelection)
        {
            this.unitSelection = unitSelection;
        }

        public IUnitAction Get(Actions action)
        {
            return action switch {
                Actions.SPIN => InstantiateSpin(),
                Actions.MOVE => InstantiateMove(),
                _ => throw new NotImplementedException(),
            };
        }

        private IUnitAction InstantiateMove()
        {
            throw new NotImplementedException();
        }

        private IUnitAction InstantiateSpin()
        {
            return new SpinUnitAction();
        }
    }
}