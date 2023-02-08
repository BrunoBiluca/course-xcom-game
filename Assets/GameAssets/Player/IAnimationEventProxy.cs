namespace GameAssets
{
    public interface IAnimationEventProxy<T> where T : struct, System.Enum
    {
        void TriggerAnimationEvent(T eventName);
    }
}
