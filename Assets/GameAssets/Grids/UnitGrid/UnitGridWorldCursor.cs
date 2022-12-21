using System;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.WorldCursors;

namespace GameAssets
{
    public class UnitGridWorldCursor : GridWorldCursor<UnitValue>
    {
        private UnitWorldGridManager gridManager;

        public event Action OnAvaiableCellSecondaryClicked;

        public void Setup(
            IRaycastHandler raycastHandler, 
            IWorldGridXZ<UnitValue> worldGrid,
            UnitWorldGridManager gridManager
        )
        {
            Setup(raycastHandler, worldGrid);
            this.gridManager = gridManager;

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
