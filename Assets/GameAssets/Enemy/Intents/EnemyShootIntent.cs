using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;

namespace GameAssets
{
    public class EnemyShootIntent : IAPIntent
    {
        private readonly ICharacterUnit unit;
        private readonly Vector3 position;
        private readonly UnitWorldGridManager gridManager;
        private readonly ProjectileFactory projectileFactory;

        public int ActionPointsCost { get; set; } = 1;

        public bool ExecuteImmediatly => true;

        public EnemyShootIntent(
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

        public IAction Create()
        {
            return new ShootAction(unit, position, gridManager, projectileFactory);
        }
    }
}
