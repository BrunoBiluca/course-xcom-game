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
                new InteractIntent(unitSelection, gridManager, worldCursor) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.INTERACT)
                },
                UnitWorldGridManager.GridState.Interact,
                gridManager.Validator()
                    .WithRange(
                        unitSelection.CurrentUnit.Transform.Position,
                        unitSelection.CurrentUnit.UnitConfig.InteractRange
                    )
                    .WhereUnitIs<IInteractableUnit>()
            );
        }

        private GridUnitAction InstantiateMelee()
        {
            return new GridUnitAction(
                new MeleeAttackIntent(gridManager, unitSelection, worldCursor) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.MELEE)
                },
                UnitWorldGridManager.GridState.Attack,
                gridManager.Validator()
                    .WithRange(
                        unitSelection.CurrentUnit.Transform.Position,
                        unitSelection.CurrentUnit.UnitConfig.MeleeRange
                    )
                    .WhereUnit((unit) => {
                        if(unit is not ICharacterUnit characterUnit)
                            return false;

                        return DamageableLayerManager.I
                            .LayerCanDamage(
                                unitSelection.CurrentUnit.Damageable.Layer,
                                characterUnit.Damageable.Layer
                            );
                    })
            );
        }

        private GridUnitAction InstantiateGrenadeThrow()
        {
            return new GridUnitAction(
                new ThrowGrenadeIntent(
                    gridManager, unitSelection, worldCursor, grenadeFactory
                ) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.GRENADE)
                },
                UnitWorldGridManager.GridState.Attack,
                gridManager.Validator()
                    .WithRange(
                        unitSelection.CurrentUnit.transform.position,
                        unitSelection.CurrentUnit.UnitConfig.GrenadeRange
                    )
            );
        }

        private GridUnitAction InstantiateShoot()
        {
            return new GridUnitAction(
                new ShootActionIntent(
                    unitSelection,
                    worldCursor,
                    gridManager,
                    projectileFactory
                ) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.SHOOT)
                },
                UnitWorldGridManager.GridState.Attack,
                gridManager.Validator()
                    .WithRange(
                        unitSelection.CurrentUnit.Transform.Position,
                        unitSelection.CurrentUnit.UnitConfig.ShootRange
                    )
                    .WhereUnit((unit) => {
                        if(unit is not ICharacterUnit characterUnit)
                            return false;

                        return DamageableLayerManager.I
                            .LayerCanDamage(
                                unitSelection.CurrentUnit.Damageable.Layer,
                                characterUnit.Damageable.Layer
                            );
                    })
            );
        }

        private GridUnitAction InstantiateMove()
        {
            return new GridUnitAction(
                new MoveActionIntent(unitSelection, worldCursor, gridManager) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.MOVE)
                },
                UnitWorldGridManager.GridState.None,
                gridManager.Validator()
                    .WhereIsEmpty()
                    .WithRange(
                        unitSelection.CurrentUnit.transform.position,
                        unitSelection.CurrentUnit.UnitConfig.MovementRange
                    )
            );
        }

        private GridUnitAction InstantiateSpin()
        {
            return new GridUnitAction(
                new SpinActionIntent(unitSelection) {
                    ActionPointsCost = actionPointsConfig.GetCost(UnitActionsEnum.SPIN)
                },
                UnitWorldGridManager.GridState.None,
                gridManager.Validator()
            );
        }
    }
}