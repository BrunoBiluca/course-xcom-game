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
            if(!TryFindClosestCharacter(out Vector3 characterPos))
                return false;

            Debug.Log("Try to move to character on: " + characterPos);

            if(!TryEvalMovePosition(characterPos, out Vector3 movePosition))
                return false;

            context.TargetPosition = movePosition;
            return true;
        }

        private bool TryFindClosestCharacter(out Vector3 characterPos)
        {
            characterPos = default;
            var minDistance = float.MaxValue;

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
            var minDistance = float.MaxValue;
            foreach(var centerPosition in gridManager.GetCellsPositions(validations))
            {
                var distance = Vector3.Distance(characterPos, centerPosition);

                Debug.Log(centerPosition + ": " + distance);

                if(distance < minDistance)
                {
                    minDistance = distance;
                    movePosition = centerPosition;
                }
            }

            return minDistance < float.MaxValue;
        }
    }
}
