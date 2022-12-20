using UnityFoundation.HealthSystem;

namespace GameAssets
{

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