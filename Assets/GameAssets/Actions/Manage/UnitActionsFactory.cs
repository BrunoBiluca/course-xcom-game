using System;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public sealed class UnitActionsFactory
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridXZManager gridManager;

        public enum Actions
        {
            MOVE,
            SPIN,
            SHOOT
        }

        public UnitActionsFactory(
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            UnitWorldGridXZManager worldGrid
        )
        {
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.gridManager = worldGrid;
        }

        public IUnitAction Get(Actions action)
        {
            return action switch {
                Actions.SPIN => InstantiateSpin(),
                Actions.MOVE => InstantiateMove(),
                Actions.SHOOT => InstantiateShoot(),
                _ => throw new NotImplementedException(),
            };
        }

        private IUnitAction InstantiateShoot()
        {
            return new ShootAction(unitSelection.CurrentUnit, worldCursor, gridManager);
        }

        private IUnitAction InstantiateMove()
        {
            return new MoveUnitAction(unitSelection.CurrentUnit, worldCursor, gridManager);
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