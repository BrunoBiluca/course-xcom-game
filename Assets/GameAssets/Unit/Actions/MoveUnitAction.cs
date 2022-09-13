using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class MoveUnitAction
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

        public bool IsDoable()
        {
            if(!worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos))
                return false;

            if(!gridManager.IsCellAvailable(pos))
                return false;

            return true;
        }

        public void Do()
        {
            if(!IsDoable())
                return;

            worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos);
            navegationAgent.SetDestination(pos);
        }

        public void ApplyValidation()
        {
            gridManager.SetRangeValidation(unit.Transform.Position, unit.MoveDistance);
        }
    }
}
