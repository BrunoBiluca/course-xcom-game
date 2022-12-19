using System;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public sealed class UnitInRangeValidationIntent : IGridValidationIntent
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly Func<UnitConfigTemplate, int> rangeProperty;
        private readonly Func<IUnit, bool> unitValidation;

        public UnitInRangeValidationIntent(
            UnitSelectionMono unitSelection,
            Func<UnitConfigTemplate, int> rangeProperty,
            Func<IUnit, bool> unitValidation
        )
        {
            this.unitSelection = unitSelection;
            this.rangeProperty = rangeProperty;
            this.unitValidation = unitValidation;
        }

        public void Validate(ref UnitWorldGridValidator validator)
        {
            validator
                .WithRange(
                    unitSelection.CurrentUnit.Transform.Position,
                    rangeProperty(unitSelection.CurrentUnit.UnitConfigTemplate)
                )
                .WhereUnit((unit) => unitValidation(unit));
        }
    }

    public sealed class DirectDamageValidationIntent : IGridValidationIntent
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly DamageableLayerManager damageableLayerManager;

        public DirectDamageValidationIntent(
            UnitSelectionMono unitSelection,
            DamageableLayerManager damageableLayerManager
        )
        {
            this.unitSelection = unitSelection;
            this.damageableLayerManager = damageableLayerManager;
        }

        public void Validate(ref UnitWorldGridValidator validator)
        {
            validator
                .WithRange(
                    unitSelection.CurrentUnit.Transform.Position,
                    unitSelection.CurrentUnit.UnitConfigTemplate.ShootRange
                )
                .WhereUnit((unit) => {
                    if(unit is not ICharacterUnit characterUnit)
                        return false;

                    return damageableLayerManager
                        .LayerCanDamage(
                            unitSelection.CurrentUnit.Damageable.Layer,
                            characterUnit.Damageable.Layer
                        );
                });
        }
    }
}