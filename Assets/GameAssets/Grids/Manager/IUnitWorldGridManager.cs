using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public interface IUnitWorldGridManager
    {
        IWorldGridXZ<UnitValue> Grid { get; }
        IEnumerable<GridCellXZ<UnitValue>> GetAllAvailableCells();
        IEnumerable<Vector3> GetAllAvailableCellsPositions();
        List<IUnit> Units { get; }
        UnitWorldGridManager.GridState State { get; set; }

        List<IUnit> GetUnitsInRange(Vector3 center, int range);

        bool IsCellAvailable(Vector3 position);
        UnitWorldGridValidator Validator();
        void ResetValidation();
    }
}