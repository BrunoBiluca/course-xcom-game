using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class GridWorldCursor : MonoBehaviour, IWorldCursor
    {
        [SerializeField] private GridXZMono grid;

        private IRaycastHandler raycastHandler;

        public Optional<Vector3> WorldPosition { get; private set; }

        public Optional<Vector2> ScreenPosition { get; private set; }

        public event Action OnClick;
        public event Action OnSecondaryClick;

        public void Awake()
        {
            ScreenPosition = Optional<Vector2>.None();
            WorldPosition = Optional<Vector3>.None();

            raycastHandler = new RaycastHandler(new CameraDecorator(Camera.main));
        }

        public void Update()
        {
            ScreenPosition = Optional<Vector2>.Some(Mouse.current.position.ReadValue());

            EvaluateWorldPosition();

            if(Mouse.current.leftButton.wasPressedThisFrame)
                OnClick?.Invoke();

            if(Mouse.current.rightButton.wasPressedThisFrame)
                OnSecondaryClick?.Invoke();
        }

        private void EvaluateWorldPosition()
        {
            var worldPosition = raycastHandler
                .GetWorldPosition(ScreenPosition.Get(), LayerMask.GetMask("Floor"));

            if(!worldPosition.IsPresentAndGet(out Vector3 pos))
            {
                WorldPosition = Optional<Vector3>.None();
                return;
            }

            WorldPosition = Optional<Vector3>.Some(grid.GetCellCenterPosition(pos));
        }
    }
}
