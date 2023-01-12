using System;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.Extensions;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;
using UnityFoundation.ResourceManagement;
using UnityFoundation.UI.Components;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class TrooperUnit :
        BilucaMono,
        ICharacterUnit,
        ISelectable
    {
        public UnitConfig UnitConfig { get; private set; }

        [SerializeField] private GameObject projectileStart;
        public ITransform ProjectileStart { get; private set; }

        [SerializeField] public GameObject rightShoulderRef;

        public ITransform RightShoulder { get; private set; }

        public IAnimatorController AnimatorController { get; private set; }
        public INavegationAgent TransformNav { get; private set; }

        public bool IsSelected { get; private set; }

        public string Name => UnitConfig.Name;

        public IHealthSystem HealthSystem { get; private set; }
        public IDamageable Damageable => HealthSystem;

        public IAPActor Actor => unitActionsManager;

        public UnitFactions Faction => UnitFactions.Player;

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

            AnimatorController = GetComponent<UnitAnimatorController>();

            HealthSystem = gameObject.GetComponent<HealthSystemMono>();
            HealthSystem.Setup(10);

            var healthController = new HealthSystemController(HealthSystem);
            healthController.AddHealthBar(transform.FindComponent<IHealthBar>("health_bar"));

            ProjectileStart = new TransformDecorator(projectileStart.transform);

            RightShoulder = new TransformDecorator(rightShoulderRef.transform);
        }

        public void Setup(
            UnitConfig unitConfigTemplate,
            UnitGridWorldCursor worldCursor
        )
        {
            this.worldCursor = worldCursor;

            UnitConfig = unitConfigTemplate;
            unitActionsManager = new APActor(
                new FiniteResourceManager(UnitConfig.MaxActionPoints, true)
            );
            Actor.OnCantExecuteAction += InvokeCantExecuteAction;
            Obj.OnObjectDestroyed += () => Actor.OnCantExecuteAction -= InvokeCantExecuteAction;
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
    }
}