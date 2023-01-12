using System;

namespace GameAssets
{
    public sealed class UnitInRangeValidationIntent : IGridValidationIntent
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly Func<UnitConfig, int> rangeProperty;
        private readonly Func<IUnit, bool> unitValidation;

        public UnitInRangeValidationIntent(
            UnitSelectionMono unitSelection,
            Func<UnitConfig, int> rangeProperty,
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
                    rangeProperty(unitSelection.CurrentUnit.UnitConfig)
                )
                .WhereUnit((unit) => unitValidation(unit));
        }
    }
}