using GameAssets.ActorSystem;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public sealed class GridUnitAction
    {
        private UnitWorldGridValidator validator;

        public IAPActionIntent Intent { get; }

        public GridUnitAction(
            UnitWorldGridManager gridManager,
            IGridValidationIntent validationIntent,
            IAPActionIntent intent
        )
        {
            Intent = intent;
            validator = new UnitWorldGridValidator(gridManager);

            validationIntent.Validate(ref validator);
        }

        public void ApplyValidation()
        {
            validator.Apply();
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

    public sealed class InRangeValidationIntent : IGridValidationIntent
    {
        private UnitSelectionMono unitSelection;

        public InRangeValidationIntent(
            UnitSelectionMono unitSelection
        )
        {
            this.unitSelection = unitSelection;
        }

        public void Validate(ref UnitWorldGridValidator validator)
        {
            validator
                .WhereIsEmpty()
                .WithRange(
                    unitSelection.CurrentUnit.transform.position,
                    unitSelection.CurrentUnit.UnitConfigTemplate.MovementRange
                );
        }
    }

    public sealed class NoValidationIntent : IGridValidationIntent
    {
        public void Validate(ref UnitWorldGridValidator validator)
        {
        }
    }
}