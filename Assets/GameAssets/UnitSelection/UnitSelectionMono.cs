using System;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitSelectionMono : MonoBehaviour
    {
        private IWorldCursor worldCursor;

        private UnitSelection unitSelection;

        public UnitMono CurrentUnit { get; private set; }

        public event Action OnUnitSelected;
        public event Action OnUnitDeselected;

        public void Setup(IWorldCursor worldCursor)
        {
            this.worldCursor = worldCursor;
        }

        public void Start()
        {
            // TODO: fazer essa configuração vir pelo Setup
            // TODO: fazer um mono que seja apenas do tipo ISelectable, para usar em futuros projetos de forma simples
            int unitLayer = LayerMask.GetMask("Unit");
            var raycastHandler = new RaycastHandler(new CameraDecorator(Camera.main));
            unitSelection = new UnitSelection(raycastHandler)
                .SetLayers(unitLayer);

            worldCursor.OnClick += TrySelectUnit;
        }

        private void TrySelectUnit()
        {
            if(!worldCursor.ScreenPosition.IsPresentAndGet(out Vector2 pos))
                return;


            unitSelection.SelectByType<UnitMono>(pos)
                .Some(u => {
                    UnityDebug.I.Log("Unit", u.name, "was selected");
                    CurrentUnit = u;
                    OnUnitSelected?.Invoke();
                })
                .OrElse(() => {
                    UnityDebug.I.Log("Unit", CurrentUnit.name, "was deselected");
                    CurrentUnit = null;
                    OnUnitDeselected?.Invoke();
                });
        }
    }
}