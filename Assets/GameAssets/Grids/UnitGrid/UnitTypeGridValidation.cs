using System;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class UnitTypeGridValidation : IGridValidation<UnitValue>
    {
        private Func<IUnit, bool> unitValidator;

        public UnitTypeGridValidation()
        {
        }

        public UnitTypeGridValidation(Func<IUnit, bool> unitValidator)
        {
            this.unitValidator = unitValidator;
        }

        public bool IsAvailable(GridCellXZ<UnitValue> cell)
        {
            if(Equals(cell.Value, default)) return false;
            if(cell.Value.Units.Count == 0) return false;

            if(unitValidator != null)
                return unitValidator.Invoke(cell.Value.Units[0]);

            return true;
        }
    }
}
