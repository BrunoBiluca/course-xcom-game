using System;

namespace GameAssets
{
    public interface IAction
    {
        event Action OnCantExecuteAction;
        event Action OnFinishAction;

        void Execute();
    }
}
