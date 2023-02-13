using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public interface IGridIntentQuery
    {
        Optional<IGridIntent> CurrentIntent { get; }
        List<GridCellXZ<UnitValue>> GetAffectedCells(Vector3 position);
        List<GridCellXZ<UnitValue>> GetAvaiableCells();
        bool IsCellAvailable(Vector3 pos);
    }
}
