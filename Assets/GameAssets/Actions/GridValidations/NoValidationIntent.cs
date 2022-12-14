namespace GameAssets
{
    public sealed class NoValidationIntent : IGridValidationIntent
    {
        public void Validate(ref UnitWorldGridValidator validator)
        {
        }
    }
}