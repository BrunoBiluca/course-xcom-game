using System;

namespace GameAssets.ActorSystem
{
    public interface IAction
    {
        event Action OnCantExecuteAction;
        event Action OnFinishAction;

        void Execute();
    }
}
