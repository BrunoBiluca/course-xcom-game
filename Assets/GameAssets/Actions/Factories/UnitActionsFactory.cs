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
                UnitActionsEnum.MELEE => InstantiateMelee(),
                UnitActionsEnum.INTERACT => InstantiateInteract(),
                _ => throw new NotImplementedException(),
            };
        }

        private GridUnitAction InstantiateInteract()
        {
            return new GridUnitAction(
                gridManager,
                new InteractIntent(unitSelection, gridManager, worldCursor){
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.INTERACT)
                },
                UnitWorldGridManager.GridState.Interact,
                new InRangeValidationIntent(unitSelection, c => c.InteractRange),
                new InteractableValidation()
            );
        }

        private GridUnitAction InstantiateMelee()
        {
            return new GridUnitAction(
                gridManager,
                new MeleeAttackIntent(gridManager, unitSelection, worldCursor) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.MELEE)
                },
                UnitWorldGridManager.GridState.Attack,
                new DirectDamageValidationIntent(
                    unitSelection,
                    DamageableLayerManager.I,
                    c => c.MeleeRange
                )
            );
        }

        private GridUnitAction InstantiateGrenadeThrow()
        {
            return new GridUnitAction(
                gridManager,
                new ThrowGrenadeIntent(
                    gridManager, unitSelection, worldCursor, grenadeFactory
                ) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.GRENADE)
                },
                UnitWorldGridManager.GridState.Attack,
                new InRangeValidationIntent(unitSelection, c => c.GrenadeRange)
            );
        }

        private GridUnitAction InstantiateShoot()
        {
            return new GridUnitAction(
                gridManager,
                new ShootActionIntent(
                    unitSelection,
                    worldCursor,
                    gridManager,
                    projectileFactory
                ) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.SHOOT)
                },
                UnitWorldGridManager.GridState.Attack,
                new DirectDamageValidationIntent(
                    unitSelection,
                    DamageableLayerManager.I,
                    c => c.ShootRange
                )
            );
        }

        private GridUnitAction InstantiateMove()
        {
            return new GridUnitAction(
                gridManager,
                new MoveActionIntent(unitSelection, worldCursor, gridManager) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.MOVE)
                },
                UnitWorldGridManager.GridState.None,
                new IsEmptyValidationIntent(),
                new InRangeValidationIntent(unitSelection, (c) => c.MovementRange)
            );
        }

        private GridUnitAction InstantiateSpin()
        {
            return new GridUnitAction(
                gridManager,
                new SpinActionIntent(unitSelection) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.SPIN)
                },
                UnitWorldGridManager.GridState.None,
                new NoValidationIntent()
            );
        }
    }
}