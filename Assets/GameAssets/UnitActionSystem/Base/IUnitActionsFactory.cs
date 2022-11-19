namespace GameAssets
{
    public interface IUnitActionsFactory<TAction> where TAction : IUnitAction
    {
        // TODO: Esse enum aqui n�o est� legal, deve ser uma estrutura que pode ser expandida.
        // talvez uma estrutura UnitActionType e o UnitActionType � gerenciado por uma classe
        TAction Get(UnitActionsEnum action);
    }
}