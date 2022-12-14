using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class ShootAnimation : IAnimationHandler
    {
        private readonly string animParam = "shoot";

        public void Handle(IAnimator animator)
        {
            animator.SetTrigger(animParam);
        }
    }
}