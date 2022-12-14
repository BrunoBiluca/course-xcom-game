using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Extensions;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;
using UnityFoundation.ResourceManagement;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class TrooperUnit :
        BilucaMono,
        IAnimationEventHandler,
        ICharacterUnit,
        ISelectable
    {
        public UnitConfigTemplate UnitConfigTemplate { get; private set; }
        public ITransform Transform { get; private set; }

        [SerializeField] private GameObject projectileStart;
        public ITransform ProjectileStart { get; private set; }

        [SerializeField] public GameObject rightShoulderRef;

        public ITransform RightShoulder { get; private set; }

        public AnimatorController AnimatorController { get; private set; }
        public INavegationAgent TransformNav { get; private set; }

        private IWorldCursor worldCursor;
        public IResourceManager ActionPoints => unitActionsManager.ActionPoints;

        public bool IsSelected { get; private set; }

        public string Name => UnitConfigTemplate.Name;

        public IHealthSystem HealthSystem { get; private set; }
        public IDamageable Damageable => HealthSystem;

        public IAPActor Actor => unitActionsManager;

        public APActor unitActionsManager;

        public event Action OnSelectedStateChange;
        public event Action OnSelected;
        public event Action OnUnselected;

        protected override void OnAwake()
        {
            Transform = new TransformDecorator(transform);
            TransformNav = new TransformNavegationAgent(
                new TransformDecorator(transform)) {
                Speed = 10f,
                StoppingDistance = 0.1f
            };

            AnimatorController = new AnimatorController(
                new AnimatorDecorator(GetComponentInChildren<Animator>())
            );

            HealthSystem = gameObject.GetComponent<HealthSystemMono>();
            HealthSystem.Setup(10);

            ProjectileStart = new TransformDecorator(projectileStart.transform);

            RightShoulder = new TransformDecorator(rightShoulderRef.transform);
        }

        public void Setup(
            UnitConfigTemplate unitConfigTemplate,
            IWorldCursor worldCursor
        )
        {
            this.worldCursor = worldCursor;

            UnitConfigTemplate = unitConfigTemplate;
            unitActionsManager = new APActor(
                new FiniteResourceManager(UnitConfigTemplate.MaxActionPoints, true)
            );
        }

        public Collider GetCollider()
        {
            return GetComponent<Collider>();
        }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            OnSelectedStateChange?.Invoke();
            if(IsSelected)
            {
                worldCursor.OnSecondaryClick -= ExecuteAction;
                worldCursor.OnSecondaryClick += ExecuteAction;
            }
            else
            {
                worldCursor.OnSecondaryClick -= ExecuteAction;
            }
        }

        private void ExecuteAction()
        {
            Actor.OnCantExecuteAction -= InvokeCantExecuteAction;
            Actor.OnCantExecuteAction += InvokeCantExecuteAction;

            Actor.Execute();
        }

        private void InvokeCantExecuteAction()
        {
            Actor.UnsetAction();
        }

        public void AnimationEventHandler(string eventName)
        {
            Debug.Log("Animation: " + eventName);

            // TODO: o AnimationController deve ser chamado para executar esse evento
        }
    }
}