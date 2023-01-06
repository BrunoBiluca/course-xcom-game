using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class EnemySpinActionIntent : IAPActionIntent
    {
        public int ActionPointsCost { get; set; } = 2;

        public bool ExecuteImmediatly => true;

        private ITransform transform;

        public EnemySpinActionIntent(ITransform transform)
        {
            this.transform = transform;
        }

        public IAction Create()
        {
            return new SpinUnitAction(AsyncProcessor.I, transform);
        }
    }
}
