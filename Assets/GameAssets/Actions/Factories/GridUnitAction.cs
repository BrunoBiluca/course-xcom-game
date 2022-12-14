namespace GameAssets
{
    public class GridUnitAction
    {
        public UnitWorldGridValidator Validator { get; private set; }

        public IAPActionIntent intent { get; }

        public GridUnitAction(
            UnitWorldGridXZManager gridManager,
            IAPActionIntent intent
        )
        {
            this.intent = intent;
            Validator = new UnitWorldGridValidator(gridManager);
        }

        public void ApplyValidation()
        {
            Validator.Apply();
        }
    }
}