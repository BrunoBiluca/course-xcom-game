using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class WorldCursor : Singleton<WorldCursor>, IWorldCursor
    {
        private RaycastHandler raycastHandler;

        public event Action OnClick;
        public event Action OnSecondaryClick;

        public Optional<Vector3> WorldPosition { get; private set; }
        public Optional<Vector2> ScreenPosition { get; private set; }

        protected override void OnAwake()
        {
            raycastHandler = new RaycastHandler(new CameraDecorator(Camera.main));

            WorldPosition = Optional<Vector3>.None();
            ScreenPosition = Optional<Vector2>.None();
        }

        public void Update()
        {
            ScreenPosition = Optional<Vector2>.Some(Mouse.current.position.ReadValue());

            WorldPosition = raycastHandler
                .GetWorldPosition(ScreenPosition.Get(), LayerMask.GetMask("Floor"));

            if(Mouse.current.leftButton.wasPressedThisFrame)
                OnClick?.Invoke();

            if(Mouse.current.rightButton.wasPressedThisFrame)
                OnSecondaryClick?.Invoke();
        }
    }
}