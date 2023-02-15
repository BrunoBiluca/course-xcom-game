using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public interface IUnitWorldGridManager
    {
        IWorldGridXZ<UnitValue> Grid { get; }
        List<IUnit> Units { get; }
        IEnumerable<GridCellXZ<UnitValue>> GetAllAvailableCells();
        IEnumerable<Vector3> GetAllAvailableCellsPositions();
        List<IUnit> GetUnitsInRange(Vector3 center, int range);
        bool IsCellAvailable(Vector3 position);

        GridValidator Validator();
        IEnumerable<GridCellXZ<UnitValue>> GetCells(IGridValidation<UnitValue>[] validations);
        IEnumerable<Vector3> GetCellsPositions(IGridValidation<UnitValue>[] validations);
    }
}