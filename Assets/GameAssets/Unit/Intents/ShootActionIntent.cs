using GameAssets.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class ShootActionIntent : IAPActionIntent
    {
        private readonly UnitSelectionMono unitSelection;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridManager gridManager;
        private readonly ProjectileFactory projectileFactory;

        public ShootActionIntent(
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            UnitWorldGridManager gridManager,
            ProjectileFactory projectileFactory
        )
        {
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
            this.projectileFactory = projectileFactory;
        }

        public int ActionPointsCost {get; set; } = 1;

        public bool ExecuteImmediatly => false;

        public IAction Create()
        {
            return new ShootAction(
                unitSelection.CurrentUnit,
                worldCursor,
                gridManager,
                projectileFactory
            );
        }
    }
}