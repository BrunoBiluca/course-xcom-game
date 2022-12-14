using GameAssets.ActorSystem;
using System;
using UnityEngine;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    // TODO: Alterar essa class para RaycastActorSelector e criar uma classe de GridActorSelector
    // a class GridActorSelector será responsável por selecionar um actor dentro de um cell
    public sealed class UnitSelectionMono
        : MonoBehaviour, IActorSelector<IAPActor>, IBilucaLoggable
    {
        private IWorldCursor worldCursor;

        private RaycastSelection unitSelection;

        public TrooperUnit CurrentUnit { get; private set; }
        public IAPActor CurrentUnitActor { get; private set; }
        public IBilucaLogger Logger { get; set; }

        public event Action OnUnitSelected;
        public event Action OnUnitUnselected;

        public void Setup(IWorldCursor worldCursor)
        {
            this.worldCursor = worldCursor;
        }

        public void Start()
        {
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

            // TODO: remover essa relação com a classe TrooperUnit
            // a relação deve ser apenas com IAPActor
            unitSelection.SelectByType<TrooperUnit>(pos)
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