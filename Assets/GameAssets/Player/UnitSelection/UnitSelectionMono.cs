using System;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.WorldCursors;

namespace GameAssets
{

    public sealed class UnitSelectionMono
        : MonoBehaviour, IActorSelector<IAPActor>, IBilucaLoggable
    {
        private IWorldCursor worldCursor;

        private RaycastSelector unitSelection;

        public TrooperUnit CurrentUnit { get; private set; }
        public IAPActor CurrentUnitActor { get; private set; }
        public IBilucaLogger Logger { get; set; }

        public event Action OnUnitSelected;
        public event Action OnUnitUnselected;

        public void Setup(IWorldCursor worldCursor)
        {
            this.worldCursor = worldCursor;
            worldCursor.OnClick += TrySelectUnit;
        }

        public void Start()
        {
            int unitLayer = LayerMask.GetMask("Unit");
            var raycastHandler = new RaycastHandler(new CameraDecorator(Camera.main));
            unitSelection = new RaycastSelector(raycastHandler)
                .SetLayers(unitLayer);
        }

        private void TrySelectUnit()
        {
            if(!worldCursor.ScreenPosition.IsPresentAndGet(out Vector2 pos))
                return;

            // TODO: remover essa relação com a classe TrooperUnit
            // a relação deve ser apenas com IAPActor
            unitSelection.Select<TrooperUnit>(pos)
                .Some(SelectUnit)
                .OrElse(UnselectUnit);
        }

        private void SelectUnit(TrooperUnit unit)
        {
            Logger?.Log("Unit", unit.name, "was selected");

            UnselectUnit();

            CurrentUnitActor = unit.Actor;
            CurrentUnit = unit;
            OnUnitSelected?.Invoke();
        }

        public void UnselectUnit()
        {
            if(CurrentUnitActor != null)
            {
                Logger?.Log("Unit was deselected");
                CurrentUnit.SetSelected(false);
                CurrentUnitActor = null;
                CurrentUnit = null;
            }
            OnUnitUnselected?.Invoke();
        }
    }
}