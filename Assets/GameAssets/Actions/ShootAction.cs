using Assets.UnityFoundation.Systems.HealthSystem;
using System;

namespace GameAssets
{
    public class ShootAction : IUnitAction
    {
        private readonly TrooperUnit tropper;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridXZManager gridManager;

        public bool ExecuteImmediatly => false;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public ShootAction(
            TrooperUnit tropper,
            IWorldCursor worldCursor,
            UnitWorldGridXZManager gridManager
        )
        {
            this.tropper = tropper;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
        }

        public void ApplyValidation()
        {
            gridManager
                .Validator()
                .WithRange(
                    tropper.Transform.Position,
                    tropper.UnitConfigTemplate.ShootRange
                )
                .WhereUnit((unit) =>
                    DamageableLayerManager.I
                        .LayerCanDamage(tropper.Damageable.Layer, unit.Damageable.Layer)
                )
                .Apply();
        }

        public void Execute()
        {
            var cellValue = gridManager.GetValueIfCellIsAvailable(worldCursor.WorldPosition.Get());

            if(cellValue == default)
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            IUnit shootedUnit = cellValue.Units[0];

            tropper.Transform.LookAt(shootedUnit.Transform.Position);

            shootedUnit.Damageable.Damage(2, tropper.Damageable.Layer);
        }
    }
}
