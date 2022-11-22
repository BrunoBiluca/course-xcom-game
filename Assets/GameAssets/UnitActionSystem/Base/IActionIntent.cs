namespace GameAssets
{
    public interface IActionIntent
    {
        bool ExecuteImmediatly { get; }

        IAction Create();
    }
}
