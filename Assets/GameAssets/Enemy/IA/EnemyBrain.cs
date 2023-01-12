using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using static UnityEngine.UI.CanvasScaler;

namespace GameAssets
{
    public enum EnemyIntents
    {
        SHOOT,
        MOVE
    }

    public class EnemyBrainContext
    {
        public Vector3 TargetPosition { get; set; }
        public Optional<IAPIntent> ChosenIntent { get; set; }
    }

    public class EnemyBrain : DecisionTree<EnemyBrainContext>, IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        public EnemyBrain(
            IAIUnit unit,
            IEnemyActionIntentFactory intentsFactory,
            IUnitWorldGridManager gridManager
        )
        {
            Context = new EnemyBrainContext();

            var tryMove = new TryMoveTowardsPlayerUnitsDecision(unit, gridManager);
            var tryShoot = new TryShootDecision(unit, gridManager);
            var takeShoot = new ChoseIntent(unit, intentsFactory, EnemyIntents.SHOOT);
            var takeMove = new ChoseIntent(unit, intentsFactory, EnemyIntents.MOVE);

            var root = tryShoot
                .SetNext(takeShoot)
                .SetFailed(
                    tryMove.SetNext(takeMove)
                );
            SetRootHandler(root);
        }

        public Optional<IAPIntent> ChooseIntent()
        {
            Context = new EnemyBrainContext();
            EvaluateDecisions(Context);
            return Context.ChosenIntent;
        }
    }
}
