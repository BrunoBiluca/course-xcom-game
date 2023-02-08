using System;
using UnityFoundation.Code.Extensions;

namespace GameAssets
{
    public interface IAnimatorController : IAnimationEventHandler
    {
        event Action<string> OnEventTriggered;

        void Play(IAnimationHandler animHandler);
    }

    public interface IAnimatorController<T> : IAnimationEventHandler<T>
    {
        event Action<T> OnEventTriggered;

        void Play(IAnimationHandler animHandler);
    }
}