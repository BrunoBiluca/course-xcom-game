using System;

namespace GameAssets
{
    public sealed class InRangeValidationIntent : IGridValidationIntent
    {
        private UnitSelectionMono unitSelection;
        private readonly Func<UnitConfig, int> property;

        public InRangeValidationIntent(
            UnitSelectionMono unitSelection,
            Func<UnitConfig, int> property
        )
        {
            this.unitSelection = unitSelection;
            this.property = property;
        }

        public void Validate(ref UnitWorldGridValidator validator)
        {
            validator
                .WithRange(
                    unitSelection.CurrentUnit.transform.position,
                    property(unitSelection.CurrentUnit.UnitConfig)
                );
        }
    }
}