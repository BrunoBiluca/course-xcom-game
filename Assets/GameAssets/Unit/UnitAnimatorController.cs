using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
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