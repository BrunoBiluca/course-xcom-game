using UnityEngine;
using UnityFoundation.Code.Extensions;

namespace GameAssets
{
    public abstract class AnimationEventProxy<T> 
        : MonoBehaviour
        , IAnimationEventProxy<T> 
        where T : struct, System.Enum
    {
        private IAnimationEventHandler<T> handler;
        [SerializeField] private GameObject goRef;

        void Start()
        {
            if(goRef == null)
                handler = gameObject.GetComponentInParent<IAnimationEventHandler<T>>();
        }

        public void TriggerAnimationEvent(T eventName)
        {
            handler.AnimationEventHandler(eventName);
        }
    }
}
