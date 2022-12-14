using GameAssets.ActorSystem;

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
}