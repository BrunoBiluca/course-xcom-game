using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class MeleeAttackIntent : IAPActionIntent
    {
        private readonly UnitWorldGridManager gridManager;
        private readonly UnitSelectionMono unitSelection;
        private readonly IWorldCursor worldCursor;

        public bool ExecuteImmediatly => false;

        public int ActionPointsCost { get; set; }

        public MeleeAttackIntent(
            UnitWorldGridManager gridManager,
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor
        )
        {
            this.gridManager = gridManager;
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
        }

        public IAction Create()
        {
            var units = gridManager.GetUnitsInRange(worldCursor.WorldPosition.Get(), 0);

            if(units[0] is not ICharacterUnit unit)
                return null;

            return new MeleeAttackAction(
                new MeleeAttackAction.Settings(4),
                unitSelection.CurrentUnit,
                unit
            );
        }
    }
}
