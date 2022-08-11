using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitMono : BilucaMonoBehaviour, ISelectable
    {
        public ITransform Transform { get; private set; }
        private TransformNavegationAgent transformNav;

        private IWorldCursor worldCursor;
        [SerializeField] private GameObject worldCursorRef;

        private GridXZMono grid;

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
            this.grid = grid;
        }

        public void Update()
        {
            transformNav.UpdateWithTime(Time.deltaTime);

            grid.TransformToGridPosition(Transform);
        }

        public Collider GetCollider()
        {
            return GetComponent<Collider>();
        }

        public void SetSelected(bool isSelected)
        {
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