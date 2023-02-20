using System;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitAnimatorController : BilucaMono, ICharacterAnimatorController
    {
        private IAnimator animator;

        public event Action<UnitAnimationEvents> OnEventTriggered;

        protected override void OnAwake()
        {
            animator = new AnimatorDecorator(GetComponentInChildren<Animator>());
        }

        public void Play(IAnimationHandler animHandler)
        {
            animHandler.Handle(animator);
        }

        public void PlayCallback(Action<IAnimator> animatorCallback)
        {
            animatorCallback(animator);
        }

        public void AnimationEventHandler(UnitAnimationEvents value)
        {
            OnEventTriggered?.Invoke(value);
        }
    }
}