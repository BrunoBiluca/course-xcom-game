namespace GameAssets
{
    public class GridUnitAction
    {
        public UnitWorldGridValidator Validator { get; private set; }

        public IAPActionIntent ActionFactory { get; }

        public GridUnitAction(
            UnitWorldGridXZManager gridManager,
            IAPActionIntent actionFactory
        )
        {
            ActionFactory = actionFactory;
            Validator = new UnitWorldGridValidator(gridManager);
        }

        public void ApplyValidation()
        {
            Validator.Apply();
        }
    }
}