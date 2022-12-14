using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
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
}