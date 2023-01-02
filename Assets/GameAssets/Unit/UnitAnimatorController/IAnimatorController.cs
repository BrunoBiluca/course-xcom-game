using System;
using UnityFoundation.Code.Extensions;

namespace GameAssets
{
    public interface IAnimatorController : IAnimationEventHandler
    {
        event Action<string> OnEventTriggered;

        void Play(IAnimationHandler animHandler);
    }
}