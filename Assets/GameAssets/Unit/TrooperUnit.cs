using Assets.UnityFoundation.Systems.HealthSystem;
using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Extensions;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.ResourceManagement;

namespace GameAssets
{
    public class TrooperUnit :
        BilucaMono,
        ISelectable,
        IAnimationEventHandler,
        IUnitActor,
        IUnit
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
        private WorldGridXZManager<UnitValue> gridManager;
        public FiniteResourceManager ActionPoints { get; private set; }

        public bool IsSelected { get; private set; }

        public string Name => UnitConfigTemplate.Name;

        public IDamageable Damageable { get; private set; }

        private Action<float> updateCallback;
        private Optional<IUnitAction> currentAction;

        public event Action OnCantExecuteAction;
        public event Action OnSelectedStateChange;

        protected override void OnAwake()
        {
            currentAction = Optional<IUnitAction>.None();

            Transform = new TransformDecorator(transform);
            TransformNav = new TransformNavegationAgent(
                new TransformDecorator(transform)) {
                Speed = 10f,
                StoppingDistance = 0.1f
            };

            AnimatorController = new AnimatorController(
                new AnimatorDecorator(GetComponentInChildren<Animator>())
            );

            Damageable = gameObject.GetComponent<HealthSystem>();
            Damageable.Setup(10);

            ProjectileStart = new TransformDecorator(projectileStart.transform);

            RightShoulder = new TransformDecorator(rightShoulderRef.transform);

            OnDestroyAction += OnDestroyHandler;
        }

        public void Setup(
            UnitConfigTemplate unitConfigTemplate,
            IWorldCursor worldCursor,
            WorldGridXZManager<UnitValue> gridManager
        )
        {
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;

            UnitConfigTemplate = unitConfigTemplate;

            ActionPoints = new FiniteResourceManager(
                UnitConfigTemplate.MaxActionPoints,
                startFull: true
            );
        }

        public void Update()
        {
            updateCallback?.Invoke(Time.deltaTime);
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

        private void OnDestroyHandler()
        {
            gridManager.ResetRangeValidation();
        }

        private void ExecuteAction()
        {
            if(!currentAction.IsPresentAndGet(out IUnitAction action))
                return;

            const uint actionPointCost = 1u;
            if(!ActionPoints.TrySubtract(actionPointCost))
            {
                InvokeCantExecuteAction();
                return;
            }

            action.OnCantExecuteAction -= InvokeCantExecuteAction;
            action.OnCantExecuteAction += InvokeCantExecuteAction;

            action.Execute();
        }

        private void InvokeCantExecuteAction()
        {
            gridManager.ResetRangeValidation();
            OnCantExecuteAction?.Invoke();
        }

        public void AnimationEventHandler(string eventName)
        {
            Debug.Log("Animation: " + eventName);

            // TODO: o AnimationController deve ser chamado para executar esse evento
        }

        public void SetAction(Optional<IUnitAction> action)
        {
            if(!action.IsPresentAndGet(out IUnitAction currentAction))
                return;

            this.currentAction = action;
            currentAction.ApplyValidation();

            if(currentAction.ExecuteImmediatly)
                ExecuteAction();
        }

        public void SetUpdateCallback(Action<float> callback)
        {
            updateCallback = callback;
        }

        public void ResetUpdateCallback()
        {
            updateCallback = null;
        }
    }
}