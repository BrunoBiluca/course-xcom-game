namespace GameAssets.ActorSystem
{
    public interface IActionIntent
    {
        bool ExecuteImmediatly { get; }

        IAction Create();
    }
}
