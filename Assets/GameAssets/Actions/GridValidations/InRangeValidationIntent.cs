namespace GameAssets
{
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
}