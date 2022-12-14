namespace GameAssets
{
    public interface IAIUnit : ICharacterUnit
    {
        void TakeActions();
        void EndActions();
    }
}
