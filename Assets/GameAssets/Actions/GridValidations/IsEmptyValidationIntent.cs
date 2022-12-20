namespace GameAssets
{
    public sealed class IsEmptyValidationIntent : IGridValidationIntent
    {
        public void Validate(ref UnitWorldGridValidator validator)
        {
            validator.WhereIsEmpty();
        }
    }
}