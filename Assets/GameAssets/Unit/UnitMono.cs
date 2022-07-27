using UnityEngine;
using UnityEngine.InputSystem;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitMono : MonoBehaviour
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

            transformNav.OnReachDestination 
                += () => animController.Play(new WalkingAnimation(false));
        }

        public void Update()
        {
            if(Mouse.current.leftButton.wasPressedThisFrame)
            {
                worldCursor
                    .WorldPosition
                    .Some(pos => {
                        transformNav.SetDestination(pos);
                        animController.Play(new WalkingAnimation(true));
                    });
            }

            transformNav.UpdateWithTime(Time.deltaTime);
        }
    }
}