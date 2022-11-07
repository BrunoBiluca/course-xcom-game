using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Extensions;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.ResourceManagement;

namespace GameAssets
{
    public class UnitMono : BilucaMonoBehaviour, ISelectable, IAnimationEventHandler, IUnitActor
    {
        public UnitConfigTemplate UnitConfigTemplate { get; private set; }
        public ITransform Transform { get; private set; }
        public AnimatorController AnimatorController { get; private set; }
        public INavegationAgent TransformNav { get; private set; }

        private IWorldCursor worldCursor;
        private WorldGridXZManager<GridUnitValue> gridManager;
        public FiniteResourceManager ActionPoints { get; private set; }

        public bool IsSelected { get; private set; }

        // TODO: esse gerenciamente de grid pode ser extraido para uma classe de grid unit, já que qualquer unidade, seja inimiga ou amiga, npc ou objetos deverão fazer esse processamento
        private IWorldGridXZ<GridUnitValue> grid;

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

            OnDestroyAction += OnDestroyHandler;
        }

        public void Setup(
            UnitConfigTemplate unitConfigTemplate,
            IWorldCursor worldCursor,
            WorldGridXZManager<GridUnitValue> gridManager
        )
        {
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
            grid = gridManager.Grid;

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