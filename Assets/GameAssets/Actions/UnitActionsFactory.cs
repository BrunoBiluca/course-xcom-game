using System;

namespace GameAssets
{
    public sealed class UnitActionsFactory
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly IWorldCursor worldCursor;
        private readonly WorldGridXZManager<GridUnitValue> worldGrid;

        public enum Actions
        {
            MOVE,
            SPIN
        }

        public UnitActionsFactory(
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            WorldGridXZManager<GridUnitValue> worldGrid
        )
        {
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.worldGrid = worldGrid;
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
            return new MoveUnitAction(unitSelection.CurrentUnit, worldCursor, worldGrid);
        }

        private IUnitAction InstantiateSpin()
        {
            return new SpinUnitAction(
                unitSelection.CurrentUnit,
                unitSelection.CurrentUnit.Transform
            ) {
                Logger = UnityDebug.I
            };
        }
    }
}