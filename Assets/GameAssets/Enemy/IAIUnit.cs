namespace GameAssets
{
    public interface IAIUnit : IUnit
    {
        void TakeActions();
        void EndActions();
    }
}
