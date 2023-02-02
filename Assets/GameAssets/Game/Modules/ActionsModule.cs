using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class ActionsModule : IDependencyModule
    {
        public void Register(IDependencyBinder binder)
        {
            binder.Register<StepMovementAction>();

            binder.Register<MeleeAttackAction>();
            binder.Register(new MeleeAttackAction.Settings(1));

            binder.Register<SpinUnitAction>();

            binder.Register<ShootAction>();

            binder.Register<InteractAction>();

            binder.Register<ShootAction>();

            binder.Register<ThrowGrenadeAction>();
        }
    }
}
