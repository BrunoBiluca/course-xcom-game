using System;
using UnityFoundation.Code;

namespace GameAssets
{
    public interface IBaseUnitActor : IUnitActor<IUnitAction> { }

    public partial interface IUnitActor<TAction> where TAction : IUnitAction
    {
        event Action OnCantExecuteAction;

        void Execute();
        void Set(TAction action);
        void UnsetAction();
    }
}