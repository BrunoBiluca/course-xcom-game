using Assets.UnityFoundation.Systems.HealthSystem;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class EnemyUnit : BilucaMono, IUnit
    {
        public ITransform Transform { get; private set; }

        public string Name => "Enemy";

        public IDamageable Damageable { get; private set; }

        protected override void OnAwake()
        {
            Transform = new TransformDecorator(transform);

            Damageable = GetComponent<HealthSystem>();
            Damageable.Setup(6);
        }
    }
}
