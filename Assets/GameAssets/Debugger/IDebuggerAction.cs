namespace GameAssets
{
    public interface IDebuggerAction
    {
        string Name { get; }
        void Execute();
    }
}
