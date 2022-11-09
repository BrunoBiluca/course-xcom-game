using Assets.UnityFoundation.Systems.HealthSystem;
using System;

namespace GameAssets
{
    public class ShootAction : IUnitAction
    {
        private readonly TrooperUnit trooper;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridXZManager gridManager;
        private readonly ProjectileFactory projectileFactory;

        public bool ExecuteImmediatly => false;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public ShootAction(
            TrooperUnit trooper,
            IWorldCursor worldCursor,
            UnitWorldGridXZManager gridManager,
            ProjectileFactory projectileFactory
        )
        {
            this.trooper = trooper;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
            this.projectileFactory = projectileFactory;
        }

        public void ApplyValidation()
        {
            gridManager
                .Validator()
                .WithRange(
                    trooper.Transform.Position,
                    trooper.UnitConfigTemplate.ShootRange
                )
                .WhereUnit((unit) =>
                    DamageableLayerManager.I
                        .LayerCanDamage(trooper.Damageable.Layer, unit.Damageable.Layer)
                )
                .Apply();
        }

        public void Execute()
        {
            var cellValue = gridManager.GetValueIfCellIsAvailable(worldCursor.WorldPosition.Get());

            if(cellValue == default)
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            IUnit shootedUnit = cellValue.Units[0];

            trooper.Transform.LookAt(shootedUnit.Transform.Position);

            // TODO: existe uma ordem nessas execu��es
            // primeiro fazemos a anima��o
            // segundo instanciamos o projectile (depois que a anima��o envia o evento de trigger)
            // terceiro efetuamos o c�lculo do dano (depois que o projectile chega no destino)

            trooper.AnimatorController.Play(new ShootAnimation());

            // TOOD: esse projectile deveria ser invocado quando um evento de
            // anima��o da anima��o de shoot � ativado, para ai ent�o criar o projectile
            // shootAnimation.OnTrigger += Instantiate
            var proj = projectileFactory.Create(
                trooper.ProjectileStart.Position,
                shootedUnit.Transform.Position
            );

            proj.OnReachTarget += () => {
                shootedUnit.Damageable.Damage(2, trooper.Damageable.Layer);
            };
        }
    }
}
