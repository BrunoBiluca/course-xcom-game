using System;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class ShootAction : IAction, IBilucaLoggable
    {
        private readonly ICharacterUnit unit;
        private readonly Vector3 position;
        private readonly IUnitWorldGridManager gridManager;
        private readonly IProjectileFactory projectileFactory;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        private IDamageableUnit targetUnit;

        public IBilucaLogger Logger { get; set; }

        public ShootAction(
            IUnitWorldGridManager gridManager,
            ICharacterUnit unit,
            Vector3 position,
            IProjectileFactory projectileFactory
        )
        {
            this.gridManager = gridManager;
            this.unit = unit;
            this.position = position;
            this.projectileFactory = projectileFactory;
        }

        public void Execute()
        {
            Logger?.LogHighlight("Executing", nameof(ShootAction));
            var cellValue = gridManager.Grid.GetValue(position);

            if(
                cellValue == default
                || cellValue.Units.IsEmpty()
                || cellValue.Units[0] is not IDamageableUnit damageableUnit
            )
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            targetUnit = damageableUnit;

            new ActionDecorator(this)
                .LookAtTarget(
                    unit, 
                    targetUnit.Transform.Position, 
                    PlayShootAnimation
                );
        }

        private void PlayShootAnimation()
        {
            unit.AnimatorController.OnEventTriggered += HandleCharacterShotAnimationEvent;
            unit.AnimatorController.Play(new ShootAnimation());
        }

        private void HandleCharacterShotAnimationEvent(UnitAnimationEvents obj)
        {
            Logger?.Log("Handle character shot animation event");
            if(!Equals(obj, UnitAnimationEvents.SHOT))
                return;

            var proj = projectileFactory.Create(
                unit.ProjectileStart.Position,
                targetUnit.ProjectileHit.Position
            );

            proj.OnReachTarget += HandleProjectileReachTarget;
        }

        private void HandleProjectileReachTarget()
        {
            targetUnit.Damageable.Damage(
                unit.UnitConfig.ShootDamage,
                unit.Damageable.Layer
            );

            unit.AnimatorController.OnEventTriggered -= HandleCharacterShotAnimationEvent;

            Logger?.LogHighlight("Finish", nameof(ShootAction));
            OnFinishAction?.Invoke();
        }
    }
}
