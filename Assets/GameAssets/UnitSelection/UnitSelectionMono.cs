using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitSelectionMono : MonoBehaviour
    {
        private IWorldCursor worldCursor;

        private UnitSelection unitSelection;

        public void Setup(IWorldCursor worldCursor)
        {
            this.worldCursor = worldCursor;
        }

        public void Start()
        {
            var raycastHandler = new RaycastHandler(new CameraDecorator(Camera.main));
            unitSelection = new UnitSelection(raycastHandler)
                .SetLayers(LayerMask.GetMask("Unit"));

            worldCursor.OnClick += TrySelectUnit;
        }

        private void TrySelectUnit()
        {
            if(!worldCursor.ScreenPosition.IsPresentAndGet(out Vector2 pos))
                return;

            unitSelection.Select(pos);
        }
    }
}