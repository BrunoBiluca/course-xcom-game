using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;

namespace GameAssets
{
    public class MeteorAttackAction : IAction
    {
        private readonly IUnitWorldGridManager gridManager;
        private readonly AreaAttackSettings settings;
        private readonly ICharacterUnit unit;
        private readonly Vector3 targetPosition;
        private readonly IProjectileFactory projectileFactory;
        private IProjectile proj;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public MeteorAttackAction(
            IUnitWorldGridManager gridManager,
            AreaAttackSettings settings,
            ICharacterUnit unit,
            Vector3 targetPosition,
            IProjectileFactory projectileFactory
        )
        {
            this.gridManager = gridManager;
            this.settings = settings;
            this.unit = unit;
            this.targetPosition = targetPosition;
            this.projectileFactory = projectileFactory;
        }

        public void Execute()
        {
            new ActionDecorator(this)
                .LookAtTarget(unit, targetPosition, InstantiateMeteor);
        }

        private void InstantiateMeteor()
        {
            unit.AnimatorController.OnEventTriggered += HandleCasting;
            unit.AnimatorController.PlayCallback(a => a.SetTrigger("cast"));
        }

        private void HandleCasting(UnitAnimationEvents obj)
        {
            if(!Equals(obj, UnitAnimationEvents.SHOT))
                return;

            unit.AnimatorController.OnEventTriggered -= HandleCasting;

            proj = projectileFactory.Create(
                targetPosition + Vector3.up * 2f,
                targetPosition.WithY(-0.5f),
                1.5f
            );
            proj.OnReachTarget += HandleProjectileReachTarget;
        }

        private void HandleProjectileReachTarget()
        {
            proj.OnReachTarget -= HandleProjectileReachTarget;

            var units = gridManager.GetUnitsInRange(targetPosition, settings.ExplosionRange);

            CameraManager.I.ShakeCamera();

            foreach(var character in units.OfType<IDamageableUnit>())
                character.Damageable.Damage(settings.Damage, null);

            foreach(var obj in units.OfType<IDestroyableUnit>())
                obj.Destroy();

            OnFinishAction?.Invoke();
        }
    }
}
