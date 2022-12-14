using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class UnitWorldGridValidator
    {
        private readonly UnitWorldGridXZManager gridManager;

        private List<IGridValidation<UnitValue>> gridValidations;

        public UnitWorldGridValidator(UnitWorldGridXZManager gridManager)
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
            gridManager.ApplyValidator(gridValidations.ToArray());
        }
    }
}
