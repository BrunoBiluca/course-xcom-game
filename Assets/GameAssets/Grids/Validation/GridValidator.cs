using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityFoundation.Code.Grid;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public class GridValidator
    {
        private readonly IUnitWorldGridManager gridManager;

        private readonly List<IGridValidation<UnitValue>> gridValidations = new();

        public GridValidator(IUnitWorldGridManager gridManager)
        {
            this.gridManager = gridManager;
        }

        public GridValidator WhereIsNotEmpty()
        {
            gridValidations.Add(
                new AndValidation<UnitValue>(
                    new NotEmptyCellGridValidation<UnitValue>(),
                    new UnitTypeGridValidation(u => u.IsBlockable)
                )
            );
            return this;
        }

        public GridValidator WhereIsEmpty()
        {
            gridValidations.Add(
                new OrValidation<UnitValue>(
                    new EmptyCellGridValidation<UnitValue>(),
                    new UnitTypeGridValidation(u => !u.IsBlockable)
                )
            );
            return this;
        }

        public GridValidator WhereCell(Vector3 position)
        {
            var currCell = gridManager.Grid.GetCell(position);
            gridValidations.Add(new RangeGridValidation<UnitValue>(currCell, 0));
            return this;
        }

        public GridValidator WithRange(Vector3 position, int range)
        {
            var currCell = gridManager.Grid.GetCell(position);
            gridValidations.Add(new RangeGridValidation<UnitValue>(currCell, range));
            return this;
        }

        public GridValidator WithDirectRange(Vector3 position, int range)
        {
            var currCell = gridManager.Grid.GetCell(position);
            gridValidations.Add(new UnitDirectRangeValidation(gridManager.Grid, currCell, range));
            return this;
        }

        public GridValidator WhereIsUnit()
        {
            gridValidations.Add(new UnitTypeGridValidation());
            return this;
        }

        public GridValidator WhereUnit(Func<IUnit, bool> unitValidator)
        {
            gridValidations.Add(new UnitTypeGridValidation(unitValidator));
            return this;
        }

        public GridValidator WhereCanDamageUnit(DamageableLayer layer)
        {
            gridValidations.Add(new DamageableUnitGridValidation(layer));
            return this;
        }

        public GridValidator WhereUnitIs<T>() where T : IUnit
        {
            gridValidations.Add(new UnitTypeGridValidation(u => u is T));
            return this;
        }

        public void Apply()
        {
            Apply(GridIntentType.Movement);
        }

        public void Apply(GridIntentType state)
        {
            gridManager.State = state;
            //gridManager.ApplyValidator(gridValidations.ToArray());
        }

        public IGridValidation<UnitValue>[] Build()
        {
            return gridValidations.ToArray();
        }
    }
}
