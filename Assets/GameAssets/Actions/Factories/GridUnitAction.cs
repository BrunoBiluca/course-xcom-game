﻿using UnityFoundation.CharacterSystem.ActorSystem;

namespace GameAssets
{
    public sealed class GridUnitAction
    {
        private UnitWorldGridValidator validator;
        private readonly UnitWorldGridManager.GridState state;

        public IAPIntent Intent { get; }

        public GridUnitAction(
            UnitWorldGridManager gridManager,
            IAPIntent intent,
            UnitWorldGridManager.GridState state,
            params IGridValidationIntent[] validationIntents
        )
        {
            Intent = intent;
            this.state = state;
            validator = new UnitWorldGridValidator(gridManager);

            foreach(var validation in validationIntents)
            {
                validation.Validate(ref validator);
            }
            
        }

        public void ApplyValidation()
        {
            validator.Apply(state);
        }
    }
}