using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;
using UnityFoundation.Physics3D;
using UnityFoundation.ResourceManagement;

namespace GameAssets
{
    public class EnemyUnit : BilucaMono, IUnit
    {
        [SerializeField] private GameObject ragdoll;

        [SerializeField] private Transform root;

        public ITransform Transform { get; private set; }

        public string Name => "Enemy";

        public IHealthSystem HealthSystem { get; private set; }

        public IDamageable Damageable => HealthSystem;

        public IAPActor Actor { get; private set; }

        public UnitConfigTemplate UnitConfigTemplate { get; private set; }

        public ITransform RightShoulder { get; private set; }

        public AnimatorController AnimatorController { get; private set; }

        public ITransform ProjectileStart { get; private set; }

        public event Action OnActionFinished;

        protected override void OnAwake()
        {
            Transform = new TransformDecorator(transform);

            HealthSystem = GetComponent<HealthSystemMono>();
            HealthSystem.Setup(6);
            HealthSystem.OnDied += DieHandler;

            Actor = new APActor(new FiniteResourceManager(4, true));
        }

        public void Setup(
            UnitConfigTemplate unitConfigTemplate
        )
        {
            UnitConfigTemplate = unitConfigTemplate;
        }

        private void DieHandler()
        {
            var ragdollHandler = Instantiate(ragdoll, transform.position, transform.rotation)
                .GetComponent<RagdollHandler>();

            ragdollHandler.Setup(new TransformDecorator(root));
        }

        public void TakeAction()
        {
            TakeSpinAction();
        }

        public void TakeSpinAction()
        {
            if(Actor.ActionPoints.CurrentAmount == 0)
            {
                UnityDebug.I.LogHighlight(nameof(EnemyUnit), "finished take actions");
                OnActionFinished?.Invoke();
                return;
            }

            var action = new EnemySpinActionIntent(Transform);
            Actor.Set(action);

            Actor.OnActionFinished -= TakeSpinAction;
            Actor.OnActionFinished += TakeSpinAction;
        }
    }
}
