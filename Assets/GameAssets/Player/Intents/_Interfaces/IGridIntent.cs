using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public interface IGridIntent : IAPIntent
    {
        public GridIntentType IntentType { get; }
        GridValidator AffectedValidation(GridValidator validator, Vector3 position);
        GridValidator AvaiableValidation(GridValidator validator);
    }
}

