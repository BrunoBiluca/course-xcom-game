using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class InteractIntent : IAPIntent
    {
        private readonly UnitSelectionMono unitSelector;
        private readonly IUnitWorldGridManager gridManager;
        private readonly IWorldCursor worldCursor;

        public bool ExecuteImmediatly => false;

        public int ActionPointsCost { get; set; }

        public InteractIntent(
            UnitSelectionMono unitSelector,
            IUnitWorldGridManager gridManager,
            IWorldCursor worldCursor
        )
        {
            this.unitSelector = unitSelector;
            this.gridManager = gridManager;
            this.worldCursor = worldCursor;
        }

        public IAction Create()
        {
            var interactable = gridManager.GetUnitsInRange(
                worldCursor.WorldPosition.Get(),
                unitSelector.CurrentUnit.UnitConfig.InteractRange
            )
                .OfType<IInteractableUnit>()
                .First();
            return new InteractAction(interactable);
        }
    }
}
