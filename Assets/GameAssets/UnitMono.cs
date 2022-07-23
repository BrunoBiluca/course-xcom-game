using UnityEngine;
using UnityEngine.InputSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitMono : MonoBehaviour
    {
        private MoveHandler moveHandler;
        private CameraPositionHelper cameraPositionHelper;

        public void Awake()
        {
            moveHandler = new MoveHandler(new TransformDecorator(transform)) {
                Speed = 10f,
                StoppingDistance = 0.1f
            };

            cameraPositionHelper = new CameraPositionHelper(Camera.main);
        }

        public void Update()
        {
            if(Mouse.current.leftButton.wasPressedThisFrame)
            {
                var worldPosition = cameraPositionHelper
                    .GetWorldPosition(Mouse.current.position.ReadValue());

                worldPosition.Some(position => moveHandler.SetDestination(position));
            }

            moveHandler.UpdateWithTime(Time.deltaTime);
        }
    }
}