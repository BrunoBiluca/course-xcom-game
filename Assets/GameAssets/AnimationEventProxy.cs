using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets
{
    public class AnimationEventProxy : MonoBehaviour
    {
        private IAnimationEventHandler handler;
        [SerializeField] private GameObject goRef;

        void Start()
        {
            if(goRef == null)
                handler = gameObject.GetComponentInParent<IAnimationEventHandler>();
        }

        public void TriggerAnimationEvent(string eventName)
        {
            handler.AnimationEventHandler(eventName);
        }
    }
}
