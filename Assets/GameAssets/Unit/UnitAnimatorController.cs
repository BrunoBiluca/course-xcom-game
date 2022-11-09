using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    // TODO: Esse controller tem que garantir quais são as animations que foram registradas para esse animator.
    // Tipo garantir que apenas animações que são construidas a partir da factory dele sejam passadas no animationHandler
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