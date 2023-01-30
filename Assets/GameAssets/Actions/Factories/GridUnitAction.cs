using UnityFoundation.CharacterSystem.ActorSystem;

namespace GameAssets
{
    public sealed class GridUnitAction
    {
        private UnitWorldGridValidator validator;
        private readonly UnitWorldGridManager.GridState state;

        public IAPIntent Intent { get; }

        public GridUnitAction(
            IAPIntent intent,
            UnitWorldGridManager.GridState state,
            UnitWorldGridValidator validator
        )
        {
            Intent = intent;
            this.state = state;
            this.validator = validator;
        }

        public void ApplyValidation()
        {
            validator.Apply(state);
        }
    }
}