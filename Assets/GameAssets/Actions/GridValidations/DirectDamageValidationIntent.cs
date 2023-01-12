using System;
using UnityFoundation.HealthSystem;

namespace GameAssets
{

    public sealed class DirectDamageValidationIntent : IGridValidationIntent
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly DamageableLayerManager damageableLayerManager;
        private readonly Func<UnitConfig, int> property;

        public DirectDamageValidationIntent(
            UnitSelectionMono unitSelection,
            DamageableLayerManager damageableLayerManager,
            Func<UnitConfig, int> property
        )
        {
            this.unitSelection = unitSelection;
            this.damageableLayerManager = damageableLayerManager;
            this.property = property;
        }

        public void Validate(ref UnitWorldGridValidator validator)
        {
            validator
                .WithRange(
                    unitSelection.CurrentUnit.Transform.Position,
                    property(unitSelection.CurrentUnit.UnitConfig)
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