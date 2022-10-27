using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Extensions;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitMono : BilucaMonoBehaviour, ISelectable, IAnimationEventHandler, IUnitActor
    {
        [field: SerializeField] public UnitConfigTemplate UnitConfigTemplate { get; private set; }
        public ITransform Transform { get; private set; }
        public AnimatorController AnimatorController { get; private set; }
        public INavegationAgent TransformNav { get; private set; }

        private IWorldCursor worldCursor;
        private WorldGridXZManager<GridUnitValue> gridManager;

        // TODO: esse gerenciamente de grid pode ser extraido para uma classe de grid unit, já que qualquer unidade, seja inimiga ou amiga, npc ou objetos deverão fazer esse processamento
        private IWorldGridXZ<GridUnitValue> grid;

        private Vector3 currentGridCellPos;
        private Action<float> updateCallback;
        private Optional<IUnitAction> currentAction;

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
            IWorldCursor worldCursor,
            WorldGridXZManager<GridUnitValue> gridManager
        )
        {
            this.worldCursor = worldCursor;
            this.gridManager = gridManager;
            grid = gridManager.Grid;
        }

        public void Update()
        {
            updateCallback?.Invoke(Time.deltaTime);

            UpdateGridPosition();
        }

        private void UpdateGridPosition()
        {
            // TODO: Atualizar o grid position não deveria ser da unidade e sim de uma estrutura paralela ao grid
            if(grid == null) return;

            var newGridPos = grid.GetCellWorldPosition(Transform.Position);
            if(currentGridCellPos != newGridPos)
            {
                grid.TryUpdateValue(currentGridCellPos, (val) => val.Remove(Transform));
                grid.TryUpdateValue(newGridPos, (val) => val.Add(Transform));
                currentGridCellPos = newGridPos;
            }
        }

        public Collider GetCollider()
        {
            return GetComponent<Collider>();
        }

        public void SetSelected(bool isSelected)
        {
            // TODO: a ação do cursor será configurada de acordo com o estado da unidade
            // a unidade pode movimentar, atacar, fazer outras ações

            // TODO: transformar essa classe em Character com estados
            if(isSelected)
                worldCursor.OnSecondaryClick += ExecuteAction;
            else
                worldCursor.OnSecondaryClick -= ExecuteAction;

            transform.Find("selection_mark").gameObject.SetActive(isSelected);
        }

        private void OnDestroyHandler()
        {
            gridManager.ResetRangeValidation();
            worldCursor.OnSecondaryClick -= ExecuteAction;
        }

        private void ExecuteAction()
        {
            try
            {
                currentAction.Some(a => a.Execute());
            }
            catch(CantExecuteActionException) { }
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