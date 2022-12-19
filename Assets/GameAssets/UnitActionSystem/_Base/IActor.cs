using System;
using UnityFoundation.Code;

namespace GameAssets.ActorSystem
{
    public partial interface IActor<TFactory> where TFactory : IActionIntent
    {
        event Action OnCantExecuteAction;
        event Action OnActionFinished;

        void Execute();
        void Set(TFactory action);
        void UnsetAction();
    }
}