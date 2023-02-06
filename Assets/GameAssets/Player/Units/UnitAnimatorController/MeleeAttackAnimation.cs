using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class MeleeAttackAnimation : IAnimationHandler
    {
        private readonly bool state;

        public MeleeAttackAnimation(bool state)
        {
            this.state = state;
        }

        public void Handle(IAnimator animator)
        {
            animator.SetBool("melee", state);
        }
    }
}
