using System;
using System.Linq;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;

namespace GameAssets
{
    public class ThrowGrenadeAction : IAction
    {
        private readonly IUnitWorldGridManager gridManager;
        private readonly Vector3 startPos;
        private readonly Vector3 targetPos;
        private readonly IProjectileFactory projectileFactory;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public Settings Config { get; private set; }

        public class Settings
        {
            public int Damage { get; set; } = 3;

            public int ExplosionRange { get; set; } = 1;
        }

        public ThrowGrenadeAction(
            Settings config,
            IUnitWorldGridManager gridManager,
            Vector3 startPos,
            Vector3 targetPos,
            IProjectileFactory projectileFactory
        )
        {
            Config = config;
            this.gridManager = gridManager;
            this.startPos = startPos;
            this.targetPos = targetPos;
            this.projectileFactory = projectileFactory;
        }

        public void Execute()
        {
            var projectile = projectileFactory.Create(startPos, targetPos);
            projectile.OnReachTarget += ReachTargetHandler;
        }

        private void ReachTargetHandler()
        {
            var units = gridManager.GetUnitsInRange(targetPos, Config.ExplosionRange);

            CameraManager.I.ShakeCamera();

            foreach(var character in units.OfType<IDamageableUnit>())
                character.Damageable.Damage(Config.Damage, null);

            foreach(var obj in units.OfType<IDestroyableUnit>())
                obj.Destroy();

            OnFinishAction?.Invoke();
        }
    }
}
