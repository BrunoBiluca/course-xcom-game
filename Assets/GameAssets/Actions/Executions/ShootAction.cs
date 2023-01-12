using System;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.HealthSystem;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class ShootAction : IAction
    {
        private readonly ICharacterUnit unit;
        private readonly Vector3 position;
        private readonly UnitWorldGridManager gridManager;
        private readonly ProjectileFactory projectileFactory;

        public bool ExecuteImmediatly => false;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public ShootAction(
            ICharacterUnit unit,
            Vector3 position,
            UnitWorldGridManager gridManager,
            ProjectileFactory projectileFactory
        )
        {
            this.unit = unit;
            this.position = position;
            this.gridManager = gridManager;
            this.projectileFactory = projectileFactory;
        }

        public void Execute()
        {
            var cellValue = gridManager.GetValueIfCellIsAvailable(position);

            if(cellValue == default)
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            if(cellValue.Units[0] is not ICharacterUnit shootedUnit)
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            unit.Transform.LookAt(shootedUnit.Transform.Position);

            if(unit.RightShoulder != null)
            {
                CameraManager.I.ShowActionCamera(
                    unit.RightShoulder.Position, shootedUnit.Transform.Position
                );
            }

            // TODO: existe uma ordem nessas execuções
            // primeiro fazemos a animação
            // segundo instanciamos o projectile (depois que a animação envia o evento de trigger)
            // terceiro efetuamos o cálculo do dano (depois que o projectile chega no destino)

            unit.AnimatorController.Play(new ShootAnimation());

            // TOOD: esse projectile deveria ser invocado quando um evento de
            // animação da animação de shoot é ativado, para ai então criar o projectile
            // shootAnimation.OnTrigger += Instantiate
            var proj = projectileFactory.Create(
                unit.ProjectileStart.Position,
                shootedUnit.Transform.Position
            );

            proj.OnReachTarget += () => {
                shootedUnit.Damageable.Damage(unit.UnitConfig.ShootDamage, unit.Damageable.Layer);

                if(unit.RightShoulder != null)
                {
                    CameraManager.I.HideActionCamera(1f);
                }

                OnFinishAction?.Invoke();
            };
        }
    }
}
