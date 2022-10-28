using System;

namespace GameAssets
{
    public interface IUnitAction
    {
        event Action OnCantExecuteAction;
        event Action OnFinishAction;
        bool ExecuteImmediatly { get; }

        // TODO: como nem toda ação tem validação de range,
        // talvez criar interfaces diferentes dada a ação selecionada
        void ApplyValidation();

        // TODO: talvez o execute poderia voltar o resultado da ação
        // (dados como quando terminou e se foi com sucesso ou não)
        void Execute();
    }
}
