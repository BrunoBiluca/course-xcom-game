using NUnit.Framework;
using System;
using System.Linq;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class ThrowGrenadeAction : IAction
    {
        private readonly UnitWorldGridManager gridManager;
        private readonly Vector3 startPos;
        private readonly Vector3 targetPos;
        private readonly IProjectileFactory projectileFactory;

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public Settings Config { get; set; }

        public class Settings
        {
            public int Damage { get; set; } = 3;

            public int ExplosionRange { get; set; } = 1;
        }

        public ThrowGrenadeAction(
            UnitWorldGridManager gridManager,
            Vector3 startPos,
            Vector3 targetPos,
            IProjectileFactory projectileFactory
        )
        {
            Config = new Settings();
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

            if(units.Count == 0)
            {
                OnCantExecuteAction?.Invoke();
                return;
            }

            CameraManager.I.ShakeCamera();

            foreach(var character in units.OfType<ICharacterUnit>())
                character.Damageable.Damage(3, null);

            foreach(var obj in units.OfType<IDestroyableUnit>())
                obj.Destroy();

            OnFinishAction?.Invoke();
        }
    }
}
