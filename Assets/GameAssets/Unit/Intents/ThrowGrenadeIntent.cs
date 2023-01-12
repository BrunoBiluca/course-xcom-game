using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class ThrowGrenadeIntent : IAPIntent
    {
        private readonly UnitWorldGridManager gridManager;
        private readonly UnitSelectionMono unitSelection;
        private readonly IWorldCursor worldCursor;
        private readonly IProjectileFactory projectileFactory;

        public bool ExecuteImmediatly => false;

        public int ActionPointsCost { get; set; }

        public ThrowGrenadeIntent(
            UnitWorldGridManager gridManager,
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            IProjectileFactory projectileFactory
        )
        {
            this.gridManager = gridManager;
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.projectileFactory = projectileFactory;
        }

        public IAction Create()
        {
            var targetPosition = worldCursor.WorldPosition.Get();
            unitSelection.CurrentUnit.Transform.LookAt(targetPosition);

            return new ThrowGrenadeAction(
                gridManager,
                unitSelection.CurrentUnit.ProjectileStart.Position,
                worldCursor.WorldPosition.Get(),
                projectileFactory
            );
        }
    }
}
