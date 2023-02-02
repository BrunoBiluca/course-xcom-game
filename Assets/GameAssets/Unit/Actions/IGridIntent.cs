using UnityFoundation.CharacterSystem.ActorSystem;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets
{
    public interface IGridIntent : IAPIntent
    {
        void GridValidation();
    }
}

