namespace GameAssets.ActorSystem
{
    public interface IAPActionIntent : IActionIntent
    {
        int ActionPointsCost { get; set; }
    }
}
