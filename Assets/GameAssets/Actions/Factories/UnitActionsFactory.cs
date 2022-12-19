using System;
using UnityFoundation.Code;
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
        private readonly GrenadeProjectileFactory grenadeFactory;
        private readonly ActionPointsConfig actionPointsConfig;

        public UnitActionsFactory(
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            UnitWorldGridManager gridManager,
            ProjectileFactory projectileFactory,
            GrenadeProjectileFactory grenadeFactory,
            ActionPointsConfig actionPointsConfig
        )
        {
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
            this.projectileFactory = projectileFactory;
            this.grenadeFactory = grenadeFactory;
            this.actionPointsConfig = actionPointsConfig;
        }

        public GridUnitAction Get(UnitActionsEnum action)
        {
            return action switch {
                UnitActionsEnum.SPIN => InstantiateSpin(),
                UnitActionsEnum.MOVE => InstantiateMove(),
                UnitActionsEnum.SHOOT => InstantiateShoot(),
                UnitActionsEnum.GRENADE => InstantiateGrenadeThrow(),
                _ => throw new NotImplementedException(),
            };
        }

        private GridUnitAction InstantiateGrenadeThrow()
        {
            return new GridUnitAction(
                gridManager,
                new UnitInRangeValidationIntent(
                    unitSelection,
                    c => c.GrenadeRange,
                    u => u is ICharacterUnit
                ),
                new ThrowGrenadeIntent(
                    gridManager, unitSelection, worldCursor, grenadeFactory
                ) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.GRENADE)
                }
            );
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
                new InRangeValidationIntent(unitSelection, (c) => c.MovementRange),
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