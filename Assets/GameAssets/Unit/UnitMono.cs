using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitMono : MonoBehaviour, ISelectable
    {
        private TransformNavegationAgent transformNav;
        private IWorldCursor worldCursor;
        private AnimatorController animController;

        public void Awake()
        {
            transformNav = new TransformNavegationAgent(new TransformDecorator(transform)) {
                Speed = 10f,
                StoppingDistance = 0.1f
            };

            worldCursor = WorldCursor.Instance;

            animController = new AnimatorController(
                new AnimatorDecorator(GetComponentInChildren<Animator>())
            );

            transformNav.OnReachDestination += FinishNavegation;
        }

        public void Update()
        {
            transformNav.UpdateWithTime(Time.deltaTime);
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
    }
}