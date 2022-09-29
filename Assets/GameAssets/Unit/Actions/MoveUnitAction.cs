using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class MoveUnitAction : IUnitAction
    {
        private readonly UnitMono unit;
        private readonly INavegationAgent navegationAgent;
        private readonly IWorldCursor worldCursor;
        private readonly WorldGridXZManager<GridUnitValue> gridManager;

        public MoveUnitAction(
            UnitMono unit,
            INavegationAgent navegationAgent,
            IWorldCursor worldCursor,
            WorldGridXZManager<GridUnitValue> gridManager
        )
        {
            this.unit = unit;
            this.navegationAgent = navegationAgent;
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
        }

        private void CanExecuteAction()
        {
            if(!worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos))
                throw new CantExecuteActionException();

            if(!gridManager.IsCellAvailable(pos))
                throw new CantExecuteActionException();
        }

        public void Execute()
        {
            CanExecuteAction();

            worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos);
            navegationAgent.SetDestination(pos);
        }

        public void ApplyValidation()
        {
            gridManager.SetRangeValidation(unit.Transform.Position, unit.MoveDistance);
        }
    }
}
