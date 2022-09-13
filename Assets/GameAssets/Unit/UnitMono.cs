using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitMono : BilucaMonoBehaviour, ISelectable
    {
        [SerializeField] private GameObject worldCursorRef;

        private int moveDistance = 3;

        public ITransform Transform { get; private set; }
        private IWorldCursor worldCursor;
        private WorldGridXZManager<GridUnitValue> gridManager;
        private TransformNavegationAgent transformNav;

        // TODO: esse gerenciamente de grid pode ser extraido para uma classe de grid unit, já que qualquer unidade, seja inimiga ou amiga, npc ou objetos deverão fazer esse processamento
        private IWorldGridXZ<GridUnitValue> grid;
        private Vector3 currentGridCellPos;

        private AnimatorController animController;

        protected override void OnAwake()
        {
            Transform = new TransformDecorator(transform);
            transformNav = new TransformNavegationAgent(
                new TransformDecorator(transform)) {
                Speed = 10f,
                StoppingDistance = 0.1f
            };

            worldCursor = worldCursorRef.GetComponent<IWorldCursor>();

            animController = new AnimatorController(
                new AnimatorDecorator(GetComponentInChildren<Animator>())
            );

            transformNav.OnReachDestination += FinishNavegation;

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
            transformNav.UpdateWithTime(Time.deltaTime);

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
            {
                worldCursor.OnSecondaryClick += UpdateNavegationDestination;
                gridManager.SetRangeValidation(Transform.Position, moveDistance);
            }
            else
            {
                gridManager.ResetRangeValidation();
                worldCursor.OnSecondaryClick -= UpdateNavegationDestination;
            }

            transform.Find("selection_mark").gameObject.SetActive(isSelected);
        }

        private void OnDestroyHandler()
        {
            gridManager.ResetRangeValidation();
            worldCursor.OnSecondaryClick -= UpdateNavegationDestination;
        }

        private void UpdateNavegationDestination()
        {
            if(!worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos))
                return;

            if(!gridManager.IsCellAvailable(grid.GetCell(pos)))
                return;

            transformNav.SetDestination(pos);
            animController.Play(new WalkingAnimation(true));
        }

        private void FinishNavegation()
        {
            gridManager.SetRangeValidation(Transform.Position, moveDistance);
            animController.Play(new WalkingAnimation(false));
        }
    }
}