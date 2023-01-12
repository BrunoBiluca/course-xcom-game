using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;

namespace GameAssets
{
    public class ChoseIntent : BaseDecisionHandler<EnemyBrainContext>
    {
        private readonly IAIUnit unit;
        private readonly IEnemyActionIntentFactory intentsFactory;
        private readonly EnemyIntents intent;

        public ChoseIntent(
            IAIUnit unit,
            IEnemyActionIntentFactory intentsFactory,
            EnemyIntents intent
        )
        {
            this.unit = unit;
            this.intentsFactory = intentsFactory;
            this.intent = intent;
        }

        protected override bool OnDecide(EnemyBrainContext context)
        {
            switch(intent)
            {
                case EnemyIntents.SHOOT:
                    context.ChosenIntent = Optional<IAPIntent>.Some(intentsFactory.IntentShoot(unit, context.TargetPosition));
                    break;
                case EnemyIntents.MOVE:
                    context.ChosenIntent = Optional<IAPIntent>.Some(intentsFactory.IntentMove(unit, context.TargetPosition));
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}
