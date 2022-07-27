using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public interface IAnimationHandler
    {
        void Handle(IAnimator animator);
    }

    public class WalkingAnimation : IAnimationHandler
    {
        private readonly string animName = "walking";
        private readonly bool state;

        public WalkingAnimation(bool state)
        {
            this.state = state;
        }

        public void Handle(IAnimator animator)
        {
            animator.SetBool(animName, state);
        }
    }

    public class AnimatorController
    {
        private readonly IAnimator animator;

        public AnimatorController(IAnimator animator)
        {
            this.animator = animator;
        }

        public void Play(IAnimationHandler animHandler)
        {
            animHandler.Handle(animator);
        }
    }
}