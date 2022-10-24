using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class GridWorldCursor : BilucaMonoBehaviour, IWorldCursor
    {
        private IWorldGridXZ<GridUnitValue> worldGrid;
        private IRaycastHandler raycastHandler;

        public Optional<Vector3> WorldPosition { get; private set; }

        public Optional<Vector2> ScreenPosition { get; private set; }

        public event Action OnClick;
        public event Action OnSecondaryClick;

        public void Setup(
            IRaycastHandler raycastHandler,
            IWorldGridXZ<GridUnitValue> worldGrid
        )
        {
            ScreenPosition = Optional<Vector2>.None();
            WorldPosition = Optional<Vector3>.None();
            this.raycastHandler = raycastHandler;
            this.worldGrid = worldGrid;
        }

        public void Update()
        {
            UpdateCursor();
        }

        private void UpdateCursor()
        {
            ScreenPosition = Optional<Vector2>.Some(Mouse.current.position.ReadValue());

            if(IgnoreClick()) return;

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

            try
            {
                WorldPosition = Optional<Vector3>.Some(worldGrid.GetCellCenterPosition(pos));
            }
            catch(ArgumentOutOfRangeException)
            {
                WorldPosition = Optional<Vector3>.None();
            }
        }

        private bool IgnoreClick()
        {
            Optional<Vector3>.None();
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
