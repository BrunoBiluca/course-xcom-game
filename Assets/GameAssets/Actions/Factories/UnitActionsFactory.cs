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
        private readonly UnitWorldGridXZManager gridManager;
        private readonly ProjectileFactory projectileFactory;

        public UnitActionsFactory(
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            UnitWorldGridXZManager gridManager,
            ProjectileFactory projectileFactory
        )
        {
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
            this.projectileFactory = projectileFactory;
        }

        public GridUnitAction Get(UnitActionsEnum action)
        {
            return action switch {
                UnitActionsEnum.SPIN => InstantiateSpin(),
                UnitActionsEnum.MOVE => InstantiateMove(),
                UnitActionsEnum.SHOOT => InstantiateShoot(),
                _ => throw new NotImplementedException(),
            };
        }

        private GridUnitAction InstantiateShoot()
        {
            var unitAction = new GridUnitAction(
                gridManager,
                new ShootActionIntent(unitSelection,
                    worldCursor,
                    gridManager,
                    projectileFactory
                )
            );

            unitAction.Validator
                .WithRange(
                    unitSelection.CurrentUnit.Transform.Position,
                    unitSelection.CurrentUnit.UnitConfigTemplate.ShootRange
                )
                .WhereUnit((unit) =>
                    DamageableLayerManager.I
                        .LayerCanDamage(
                            unitSelection.CurrentUnit.Damageable.Layer,
                            unit.Damageable.Layer
                        )
                );

            return unitAction;
        }

        private GridUnitAction InstantiateMove()
        {
            var action = new MoveActionIntent(unitSelection, worldCursor, gridManager);
            var unitAction = new GridUnitAction(gridManager, action);

            unitAction.Validator
                .WhereIsEmpty()
                .WithRange(
                    unitSelection.CurrentUnit.transform.position,
                    unitSelection.CurrentUnit.UnitConfigTemplate.MovementRange
                );

            return unitAction;
        }

        private GridUnitAction InstantiateSpin()
        {
            var action = new SpinActionIntent(unitSelection);

            return new GridUnitAction(gridManager, action);
        }
    }
}