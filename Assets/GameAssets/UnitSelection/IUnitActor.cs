using System;
using UnityFoundation.Code;

namespace GameAssets
{
    public interface IUnitActor
    {
        void SetAction(Optional<IUnitAction> action);

        /// <summary>
        /// Register action to be executed on actor's update
        /// </summary>
        /// <param name="callback"></param>
        void SetUpdateCallback(Action<float> callback);
        void ResetUpdateCallback();
    }
}