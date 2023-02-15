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
            var validations = gridManager
                .Validator()
                .WhereIsEmpty()
                .WithRange(unit.Transform.Position, unit.UnitConfig.MovementRange)
                .Build();

            movePosition = default;
            var maxDistance = 0f;
            foreach(var centerPosition in gridManager.GetCellsPositions(validations))
            {
                var distance = Vector3.Distance(unit.Transform.Position, centerPosition);

                if(distance > maxDistance)
                {
                    maxDistance = distance;
                    movePosition = centerPosition;
                }
            }

            return maxDistance > 0;
        }
    }
}
