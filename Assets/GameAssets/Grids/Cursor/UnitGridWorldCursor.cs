using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class UnitGridWorldCursor
        : GridWorldCursor<UnitValue>,
        IDependencySetup<IRaycastHandler, IUnitWorldGridManager, IGridIntentQuery>
    {
        private IGridIntentQuery intentSelector;

        public event Action OnAvaiableCellSecondaryClicked;

        public void Setup(
            IRaycastHandler raycastHandler,
            IUnitWorldGridManager gridManager,
            IGridIntentQuery intentSelector
        )
        {
            Setup(raycastHandler, gridManager.Grid);
            this.intentSelector = intentSelector;
        }

        public override void Enable()
        {
            base.Enable();
            OnSecondaryClick -= HandleSecondaryClick;
            OnSecondaryClick += HandleSecondaryClick;
        }

        private void HandleSecondaryClick()
        {
            if(!WorldPosition.IsPresentAndGet(out Vector3 pos))
                return;

            if(!intentSelector.IsCellAvailable(pos))
                return;

            OnAvaiableCellSecondaryClicked?.Invoke();
        }
    }
}
