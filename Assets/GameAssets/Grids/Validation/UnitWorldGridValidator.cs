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
            gridValidations.Add(
                new AndValidation<UnitValue>(
                    new NotEmptyCellGridValidation<UnitValue>(),
                    new UnitTypeGridValidation(u => u.IsBlockable)
                )
            );
            return this;
        }

        public UnitWorldGridValidator WhereIsEmpty()
        {
            gridValidations.Add(
                new OrValidation<UnitValue>(
                    new EmptyCellGridValidation<UnitValue>(),
                    new UnitTypeGridValidation(u => !u.IsBlockable)
                )
            );
            return this;
        }

        public UnitWorldGridValidator WithRange(Vector3 position, int range)
        {
            var currCell = gridManager.Grid.GetCell(position);
            gridValidations.Add(new RangeGridValidation<UnitValue>(currCell, range));
            return this;
        }

        public UnitWorldGridValidator WithDirectRange(Vector3 position, int range)
        {
            var currCell = gridManager.Grid.GetCell(position);
            gridValidations.Add(new UnitDirectRangeValidation(gridManager.Grid, currCell, range));
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

        public UnitWorldGridValidator WhereUnitIs<T>() where T : IUnit
        {
            gridValidations.Add(new UnitTypeGridValidation(u => u is T));
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
