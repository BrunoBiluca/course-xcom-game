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
        : MonoBehaviour, 
        IActorSelector<IAPActor>, 
        IDependencySetup<IWorldCursor, ISelector>,
        IBilucaLoggable
    {
        private IWorldCursor worldCursor;
        private ISelector selector;

        public PlayerUnit CurrentUnit { get; private set; }
        public IAPActor CurrentUnitActor { get; private set; }
        public IBilucaLogger Logger { get; set; }

        public event Action OnUnitSelected;
        public event Action OnUnitUnselected;

        public void Setup(IWorldCursor worldCursor, ISelector selector)
        {
            this.worldCursor = worldCursor;
            worldCursor.OnClick += TrySelectUnit;

            this.selector = selector;
        }

        private void TrySelectUnit()
        {
            if(!worldCursor.WorldPosition.IsPresentAndGet(out Vector3 pos))
            {
                UnselectUnit();
                return;
            }
                
            selector.Select<SelectableObject>(pos)
                .Some(SelectUnit)
                .OrElse(UnselectUnit);
        }

        private void SelectUnit(SelectableObject obj)
        {
            var unit = obj.SelectedReference as PlayerUnit;

            if(CurrentUnit == unit)
                return;

            UnselectUnit();
            Logger?.Log("Unit", unit.name, "was selected");

            CurrentUnitActor = unit.Actor;
            CurrentUnit = unit;
            OnUnitSelected?.Invoke();
        }

        public void UnselectUnit()
        {
            if(CurrentUnitActor != null)
            {
                Logger?.Log("Unit was deselected");
                CurrentUnitActor = null;
                CurrentUnit = null;
            }
            OnUnitUnselected?.Invoke();
        }
    }
}