using GameAssets.ActorSystem;
using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;
using UnityFoundation.Physics3D;
using UnityFoundation.ResourceManagement;
using UnityFoundation.UI.Components;

namespace GameAssets
{

    public class EnemyUnit : BilucaMono, IAIUnit
    {
        [SerializeField] private GameObject ragdoll;

        [SerializeField] private Transform root;

        public string Name => "Enemy";

        public IHealthSystem HealthSystem { get; private set; }

        public IDamageable Damageable => HealthSystem;

        public IAPActor Actor { get; private set; }

        public UnitConfigTemplate UnitConfigTemplate { get; private set; }

        public ITransform RightShoulder { get; private set; }

        public IAnimatorController AnimatorController { get; private set; }

        public ITransform ProjectileStart { get; private set; }

        public INavegationAgent TransformNav => throw new NotImplementedException();

        public event Action OnActionFinished;

        protected override void OnAwake()
        {
            HealthSystem = GetComponent<HealthSystemMono>();
            HealthSystem.Setup(6);
            HealthSystem.OnDied += DieHandler;

            var healthController = new HealthSystemController(HealthSystem);
            healthController.AddHealthBar(transform.FindComponent<IHealthBar>("health_bar"));

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

        public void TakeActions()
        {
            var brain = new EnemyBrain(this);
            brain.TakeActions();
        }

        public void EndActions()
        {
            OnActionFinished?.Invoke();
        }
    }
}
