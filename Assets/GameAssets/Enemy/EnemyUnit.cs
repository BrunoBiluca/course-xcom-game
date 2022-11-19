using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;
using UnityFoundation.Physics3D;

namespace GameAssets
{
    public class EnemyUnit : BilucaMono, IUnit
    {
        public ITransform Transform { get; private set; }

        public string Name => "Enemy";

        public IHealthSystem HealthSystem { get; private set; }

        public IDamageable Damageable => HealthSystem;

        public IAPUnitActor Actor => throw new NotImplementedException();

        public event Action OnActionFinished;

        [SerializeField] private GameObject ragdoll;

        [SerializeField] private Transform root;

        protected override void OnAwake()
        {
            Transform = new TransformDecorator(transform);

            HealthSystem = GetComponent<HealthSystemMono>();
            HealthSystem.Setup(6);

            HealthSystem.OnDied += DieHandler;
        }

        private void DieHandler()
        {
            var ragdollHandler = Instantiate(ragdoll, transform.position, transform.rotation)
                .GetComponent<RagdollHandler>();

            ragdollHandler.Setup(new TransformDecorator(root));
        }

        public void TakeAction()
        {
            var action = new SpinUnitAction(AsyncProcessor.I, Transform);
            action.OnFinishAction += () => OnActionFinished?.Invoke();
            action.Execute();
        }
    }
}
