using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitMono : BilucaMonoBehaviour, ISelectable
    {
        public ITransform Transform { get; private set; }
        private TransformNavegationAgent transformNav;

        private IWorldCursor worldCursor;
        [SerializeField] private GameObject worldCursorRef;

        // TODO: esse gerenciamente de grid pode ser extraido para uma classe de grid unit, j� que qualquer unidade, seja inimiga ou amiga, npc ou objetos dever�o fazer esse processamento
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

        public void Setup(IWorldCursor worldCursor, GridXZMono grid)
        {
            this.worldCursor = worldCursor;
            this.grid = grid.Grid;
        }

        public void Update()
        {
            transformNav.UpdateWithTime(Time.deltaTime);

            UpdateGridPosition();
        }

        private void UpdateGridPosition()
        {
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
            // TODO: a a��o do cursor ser� configurada de acordo com o estado da unidade
            // a unidade pode movimentar, atacar, fazer outras a��es
            if(isSelected)
                worldCursor.OnSecondaryClick += UpdateNavegationDestination;
            else
                worldCursor.OnSecondaryClick -= UpdateNavegationDestination;

            transform.Find("selection_mark").gameObject.SetActive(isSelected);
        }

        private void OnDestroyHandler()
        {
            worldCursor.OnSecondaryClick -= UpdateNavegationDestination;
        }

        private void UpdateNavegationDestination()
        {
            worldCursor
                .WorldPosition
                .Some(pos => {
                    transformNav.SetDestination(pos);
                    animController.Play(new WalkingAnimation(true));
                });
        }

        private void FinishNavegation()
        {
            animController.Play(new WalkingAnimation(false));
        }
    }
}