using System;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{   // TODO: Esse controller tem que garantir quais são as animations que foram registradas para esse animator.
    // Tipo garantir que apenas animações que são construidas a partir da factory dele sejam passadas no animationHandler
    public class UnitAnimatorController : BilucaMono, IAnimatorController
    {
        private IAnimator animator;

        public event Action<string> OnEventTriggered;

        protected override void OnAwake()
        {
            animator = new AnimatorDecorator(GetComponentInChildren<Animator>());
        }

        public void AnimationEventHandler(string eventName)
        {
            OnEventTriggered?.Invoke(eventName);
        }

        public void Play(IAnimationHandler animHandler)
        {
            animHandler.Handle(animator);
        }
    }
}