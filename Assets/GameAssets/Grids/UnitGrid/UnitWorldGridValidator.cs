using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class UnitWorldGridValidator
    {
        private readonly UnitWorldGridManager gridManager;

        private readonly List<IGridValidation<UnitValue>> gridValidations;

        public UnitWorldGridValidator(UnitWorldGridManager gridManager)
        {
            this.gridManager = gridManager;
            gridValidations = new List<IGridValidation<UnitValue>>();
        }

        public UnitWorldGridValidator WhereIsNotEmpty()
        {
            gridValidations.Add(new NotEmptyCellGridValidation<UnitValue>());
            return this;
        }

        public UnitWorldGridValidator WhereIsEmpty()
        {
            gridValidations.Add(new EmptyCellGridValidation<UnitValue>());
            return this;
        }

        public UnitWorldGridValidator WithRange(Vector3 position, int range)
        {
            var currCell = gridManager.Grid.GetCell(position);
            gridValidations.Add(new RangeGridValidation<UnitValue>(currCell, range));
            return this;
        }

        public UnitWorldGridValidator WhereIsUnit()
        {
            gridValidations.Add(new UnitTypeGridValidation());
            return this;
        }

        public UnitWorldGridValidator WhereUnit(Func<IUnit, bool> unitValidator)
        {
            gridValidations.Add(new UnitTypeGridValidation(unitValidator));
            return this;
        }

        public void Apply()
        {
            Apply(UnitWorldGridManager.GridState.None);
        }

        public void Apply(UnitWorldGridManager.GridState state)
        {
            gridManager.State = state;
            gridManager.ApplyValidator(gridValidations.ToArray());
        }
    }
}
