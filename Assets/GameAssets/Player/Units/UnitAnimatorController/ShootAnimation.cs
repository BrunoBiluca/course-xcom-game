using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class ShootAnimation : IAnimationHandler
    {
        private readonly string animParam = "shot";

        public void Handle(IAnimator animator)
        {
            animator.SetTrigger(animParam);
        }
    }
}