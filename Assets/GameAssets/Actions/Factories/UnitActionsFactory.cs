using System;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.HealthSystem;
using UnityFoundation.WorldCursors;

namespace GameAssets
{

    public sealed class UnitActionsFactory
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridManager gridManager;
        private readonly ProjectileFactory projectileFactory;
        private readonly ActionPointsConfig actionPointsConfig;

        public UnitActionsFactory(
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            UnitWorldGridManager gridManager,
            ProjectileFactory projectileFactory,
            ActionPointsConfig actionPointsConfig
        )
        {
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
            this.projectileFactory = projectileFactory;
            this.actionPointsConfig = actionPointsConfig;
        }

        public GridUnitAction Get(UnitActionsEnum action)
        {
            return action switch {
                UnitActionsEnum.SPIN => InstantiateSpin(),
                UnitActionsEnum.MOVE => InstantiateMove(),
                UnitActionsEnum.SHOOT => InstantiateShoot(),
                UnitActionsEnum.GRENADE => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
        }

        private GridUnitAction InstantiateShoot()
        {
            return new GridUnitAction(
                gridManager,
                new DirectDamageValidationIntent(
                    unitSelection,
                    DamageableLayerManager.I
                ),
                new ShootActionIntent(
                    unitSelection,
                    worldCursor,
                    gridManager,
                    projectileFactory
                ) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.SHOOT)
                }
            );
        }

        private GridUnitAction InstantiateMove()
        {
            return new GridUnitAction(
                gridManager,
                new InRangeValidationIntent(unitSelection),
                new MoveActionIntent(unitSelection, worldCursor, gridManager) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.MOVE)
                }
            );
        }

        private GridUnitAction InstantiateSpin()
        {
            return new GridUnitAction(
                gridManager,
                new NoValidationIntent(),
                new SpinActionIntent(unitSelection) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.SPIN)
                }
            );
        }
    }
}