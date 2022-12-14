using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class EnemyBrain
    {
        private readonly IAIUnit unit;

        public EnemyBrain(IAIUnit unit)
        {
            this.unit = unit;
        }

        public void TakeActions()
        {
            TakeAction();
        }

        private void TakeAction()
        {
            if(unit.Actor.ActionPoints.CurrentAmount == 0)
            {
                UnityDebug.I.LogHighlight(nameof(EnemyUnit), "finished take actions");
                unit.EndActions();
                return;
            }

            TakeSpinAction();
        }

        private void TakeSpinAction()
        {
            var action = new EnemySpinActionIntent(unit.Transform);
            unit.Actor.Set(action);

            unit.Actor.OnActionFinished -= TakeAction;
            unit.Actor.OnActionFinished += TakeAction;
        }
    }
}
