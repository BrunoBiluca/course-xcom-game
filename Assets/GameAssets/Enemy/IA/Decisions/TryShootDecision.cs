using UnityFoundation.Code;

namespace GameAssets
{
    public class TryShootDecision : BaseDecisionHandler<EnemyBrainContext>
    {
        private readonly IAIUnit unit;
        private readonly IUnitWorldGridManager gridManager;

        public TryShootDecision(IAIUnit unit, IUnitWorldGridManager gridManager)
        {
            this.unit = unit;
            this.gridManager = gridManager;
        }

        protected override bool OnDecide(EnemyBrainContext context)
        {
            // TODO: deve ser adicionado ao gridManager (como se fosse uma query)
            var units = gridManager.GetUnitsInRange(
                unit.Transform.Position, unit.UnitConfig.ShootRange
            );
            foreach(var gridUnit in units)
            {
                if(gridUnit.Faction != UnitFactions.Player)
                    continue;

                context.TargetPosition = gridUnit.Transform.Position;
                return true;
            }
            return false;
        }
    }
}
