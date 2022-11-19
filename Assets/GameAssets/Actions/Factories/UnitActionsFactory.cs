using System;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public sealed class UnitActionsFactory : IUnitActionsFactory<IUnitAction>
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridXZManager gridManager;
        private readonly ProjectileFactory projectileFactory;

        public UnitActionsFactory(
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            UnitWorldGridXZManager worldGrid,
            ProjectileFactory projectileFactory
        )
        {
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.gridManager = worldGrid;
            this.projectileFactory = projectileFactory;
        }

        public IUnitAction Get(UnitActionsEnum action)
        {
            return action switch {
                UnitActionsEnum.SPIN => InstantiateSpin(),
                UnitActionsEnum.MOVE => InstantiateMove(),
                UnitActionsEnum.SHOOT => InstantiateShoot(),
                _ => throw new NotImplementedException(),
            };
        }

        private IUnitAction InstantiateShoot()
        {
            return new ShootAction(
                unitSelection.CurrentUnit,
                worldCursor,
                gridManager,
                projectileFactory
            );
        }

        private IUnitAction InstantiateMove()
        {
            return new MoveUnitAction(
                unitSelection.CurrentUnit, AsyncProcessor.I, worldCursor, gridManager
            );
        }

        private IUnitAction InstantiateSpin()
        {
            return new SpinUnitAction(
                AsyncProcessor.I,
                unitSelection.CurrentUnit.Transform
            ) {
                Logger = UnityDebug.I
            };
        }
    }
}