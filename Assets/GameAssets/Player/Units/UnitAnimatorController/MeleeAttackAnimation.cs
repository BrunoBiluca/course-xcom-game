using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class MeleeAttackAnimation : IAnimationHandler
    {
        public void Handle(IAnimator animator)
        {
            animator.SetTrigger("melee");
        }
    }
}
