using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class EnemyUnit : BilucaMono, IUnit
    {
        public ITransform Transform { get; private set; }

        public string Name => "Enemy";

        public IDamageable Damageable { get; private set; }

        [SerializeField] private GameObject ragdoll;

        [SerializeField] private Transform root;

        protected override void OnAwake()
        {
            Transform = new TransformDecorator(transform);

            Damageable = GetComponent<HealthSystem>();
            Damageable.Setup(6);

            Damageable.OnDied += DieHandler;
        }

        private void DieHandler(object sender, System.EventArgs e)
        {
            var ragdollHandler = Instantiate(ragdoll, transform.position, transform.rotation)
                .GetComponent<RagdollHandler>();

            ragdollHandler.Setup(new TransformDecorator(root));
        }
    }
}
