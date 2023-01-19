namespace GameAssets
{
    public sealed class InteractableValidation : IGridValidationIntent
    {
        public void Validate(ref UnitWorldGridValidator validator)
        {
            validator.WhereUnitIs<IInteractableUnit>();
        }
    }
}