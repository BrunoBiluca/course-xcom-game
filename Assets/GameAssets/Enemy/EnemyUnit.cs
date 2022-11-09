using Assets.UnityFoundation.Systems.HealthSystem;
using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class EnemyUnit : BilucaMono, IUnit
    {
        public ITransform Transform { get; private set; }

        public string Name => "Enemy";

        public IDamageable Damageable { get; private set; }

        public event Action OnActionFinished;

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

        public void TakeAction()
        {
            var action = new SpinUnitAction(AsyncProcessor.I, Transform);
            action.OnFinishAction += () => OnActionFinished?.Invoke();
            action.Execute();
        }
    }
}
