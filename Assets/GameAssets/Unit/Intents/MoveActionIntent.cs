using UnityFoundation.Code;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public sealed class MoveActionIntent : IAPActionIntent
    {
        public bool ExecuteImmediatly => false;

        public int ActionPointsCost { get; set; } = 1;

        private readonly UnitSelectionMono unitSelection;
        private readonly IWorldCursor worldCursor;
        private readonly UnitWorldGridXZManager gridManager;

        public MoveActionIntent(
            UnitSelectionMono unitSelection,
            IWorldCursor worldCursor,
            UnitWorldGridXZManager gridManager
        )
        {
            this.unitSelection = unitSelection;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
        }

        public IAction Create()
        {
            //return new MoveUnitAction(
            //    unitSelection.CurrentUnit, AsyncProcessor.I, worldCursor, gridManager
            //);
            return new StepMoveUnitAction(
                unitSelection.CurrentUnit, gridManager, worldCursor, AsyncProcessor.I
            );
        }
    }
}