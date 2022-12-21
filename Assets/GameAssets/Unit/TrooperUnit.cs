using GameAssets.ActorSystem;
using System;
using UnityEngine;
using UnityFoundation.Code.Extensions;
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

        [SerializeField] private GameObject projectileStart;
        public ITransform ProjectileStart { get; private set; }

        [SerializeField] public GameObject rightShoulderRef;

        public ITransform RightShoulder { get; private set; }

        public AnimatorController AnimatorController { get; private set; }
        public INavegationAgent TransformNav { get; private set; }

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

        private UnitGridWorldCursor worldCursor;

        protected override void OnAwake()
        {
            TransformNav = new TransformNavegationAgent(Transform) {
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
            UnitGridWorldCursor worldCursor
        )
        {
            this.worldCursor = worldCursor;

            UnitConfigTemplate = unitConfigTemplate;
            unitActionsManager = new APActor(
                new FiniteResourceManager(UnitConfigTemplate.MaxActionPoints, true)
            );
            Actor.OnCantExecuteAction += InvokeCantExecuteAction;
            OnObjectDestroyed += () => Actor.OnCantExecuteAction -= InvokeCantExecuteAction;
        }

        public Collider GetCollider()
        {
            return GetComponent<Collider>();
        }

        public void SetSelected(bool isSelected)
        {
            UpdateSelected(isSelected);
            SubscribeExecuteActionEvent();
        }

        private void SubscribeExecuteActionEvent()
        {
            worldCursor.OnAvaiableCellSecondaryClicked -= ExecuteAction;
            if(IsSelected)
                worldCursor.OnAvaiableCellSecondaryClicked += ExecuteAction;
        }

        private void UpdateSelected(bool isSelected)
        {
            IsSelected = isSelected;
            if(IsSelected)
                OnSelected?.Invoke();
            else
                OnUnselected?.Invoke();
            OnSelectedStateChange?.Invoke();
        }

        private void ExecuteAction()
        {

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