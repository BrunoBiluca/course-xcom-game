using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitSelectionMono : MonoBehaviour
    {
        private WorldCursor worldCursor;
        private UnitSelection unitSelection;

        public void Start()
        {
            worldCursor = WorldCursor.Instance;

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