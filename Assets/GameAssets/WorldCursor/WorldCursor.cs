using UnityEngine;
using UnityEngine.InputSystem;
using UnityFoundation.Code;

namespace GameAssets
{
    public class WorldCursor : Singleton<WorldCursor>, IWorldCursor
    {
        private CameraPositionHelper cameraPositionHelper;

        public Optional<Vector3> WorldPosition { get; private set; }

        protected override void OnAwake()
        {
            cameraPositionHelper = new CameraPositionHelper(Camera.main);
        }

        public void Update()
        {
            WorldPosition = cameraPositionHelper
                .GetWorldPosition(Mouse.current.position.ReadValue());
        }
    }
}