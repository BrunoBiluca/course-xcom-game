using System;
using UnityEngine;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public sealed class UnitSelectionMono
        : MonoBehaviour, IUnitActorSelector<IAPUnitActor>
    {
        private IWorldCursor worldCursor;

        private RaycastSelection unitSelection;

        public TrooperUnit CurrentUnit { get; private set; }
        public IAPUnitActor CurrentUnitActor { get; private set; }

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
            unitSelection = new RaycastSelection(raycastHandler)
                .SetLayers(unitLayer);

            worldCursor.OnClick += TrySelectUnit;
        }

        private void TrySelectUnit()
        {
            if(!worldCursor.ScreenPosition.IsPresentAndGet(out Vector2 pos))
                return;


            unitSelection.SelectByType<TrooperUnit>(pos)
                .Some(SelectUnit)
                .OrElse(DeselectUnit);
        }

        private void SelectUnit(TrooperUnit unit)
        {
            UnityDebug.I.Log("Unit", unit.name, "was selected");

            DeselectUnit();

            CurrentUnitActor = unit.Actor;
            CurrentUnit = unit;
            OnUnitSelected?.Invoke();
        }

        private void DeselectUnit()
        {
            if(CurrentUnitActor != null)
            {
                UnityDebug.I.Log("Unit was deselected");
                CurrentUnitActor = null;
                CurrentUnit = null;
            }
            OnUnitDeselected?.Invoke();
        }
    }
}