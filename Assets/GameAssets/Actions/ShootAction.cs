using Assets.UnityFoundation.Systems.HealthSystem;
using System;

namespace GameAssets
{
    public class ShootAction : IUnitAction
    {
        private readonly TrooperUnit tropper;
        private readonly UnitWorldGridXZManager gridManager;

        public bool ExecuteImmediatly => false;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public ShootAction(
            TrooperUnit tropper,
            UnitWorldGridXZManager gridManager
        )
        {
            this.tropper = tropper;
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
            throw new NotImplementedException();
        }
    }
}
