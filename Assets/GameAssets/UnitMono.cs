using UnityEngine;
using UnityEngine.InputSystem;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitMono : MonoBehaviour
    {
        private MoveHandler moveHandler;
        private IWorldCursor worldCursor;

        public void Awake()
        {
            moveHandler = new MoveHandler(new TransformDecorator(transform)) {
                Speed = 10f,
                StoppingDistance = 0.1f
            };

            worldCursor = WorldCursor.Instance;
        }

        public void Update()
        {
            if(Mouse.current.leftButton.wasPressedThisFrame)
            {
                worldCursor.WorldPosition.Some(pos => moveHandler.SetDestination(pos));
            }

            moveHandler.UpdateWithTime(Time.deltaTime);
        }
    }
}