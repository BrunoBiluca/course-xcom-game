using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class TryMoveTowardsPlayerUnitsDecision : BaseDecisionHandler<EnemyBrainContext>
    {
        private IAIUnit unit;
        private IUnitWorldGridManager gridManager;

        public TryMoveTowardsPlayerUnitsDecision(IAIUnit unit, IUnitWorldGridManager gridManager)
        {
            this.unit = unit;
            this.gridManager = gridManager;
        }
        protected override bool OnDecide(EnemyBrainContext context)
        {
            var characterPos = Vector3.zero;
            var minDistance = float.MaxValue;

            // TODO: deve ser adicionado ao gridManager (como se fosse uma query)
            foreach(var gridUnit in gridManager.Units)
            {
                if(gridUnit.Faction != UnitFactions.Player)
                    continue;

                var distance = Vector3.Distance(
                    unit.Transform.Position, gridUnit.Transform.Position
                );

                if(distance < minDistance)
                {
                    minDistance = distance;
                    characterPos = gridUnit.Transform.Position;
                }
            }

            if(minDistance == float.MaxValue)
                return false;

            if(!TryEvalMovePosition(characterPos, out Vector3 movePosition))
                return false;

            context.TargetPosition = movePosition;
            return true;
        }

        private bool TryEvalMovePosition(Vector3 characterPos, out Vector3 movePosition)
        {
            gridManager
                .Validator()
                .WithRange(
                    unit.Transform.Position, unit.UnitConfig.MovementRange
                )
                .WhereIsEmpty()
                .Apply();

            movePosition = default;
            var maxDistance = 0f;
            foreach(var cPos in gridManager.GetAllAvailableCellsPositions())
            {
                var distance = Vector3.Distance(unit.Transform.Position, cPos);

                if(distance > maxDistance)
                {
                    maxDistance = distance;
                    movePosition = cPos;
                }
            }

            return maxDistance > 0;
        }
    }
}
