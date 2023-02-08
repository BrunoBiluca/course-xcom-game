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
        IDependencySetup<IRaycastHandler, UnitWorldGridManager>
    {
        private UnitWorldGridManager gridManager;

        public event Action OnAvaiableCellSecondaryClicked;

        public void Setup(
            IRaycastHandler raycastHandler,
            UnitWorldGridManager gridManager
        )
        {
            Setup(raycastHandler, gridManager.Grid);
            this.gridManager = gridManager;
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

            if(!gridManager.IsCellAvailable(pos))
                return;

            OnAvaiableCellSecondaryClicked?.Invoke();
        }
    }
}
