using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class GridIntentQuery : IGridIntentQuery
    {
        private readonly IGridIntentSelector intentselector;
        private readonly IUnitWorldGridManager gridManager;

        public Optional<IGridIntent> CurrentIntent => intentselector.CurrentIntent;

        public GridIntentQuery(
            IGridIntentSelector intentselector,
            IUnitWorldGridManager gridManager
        )
        {
            this.intentselector = intentselector;
            this.gridManager = gridManager;
        }

        public List<GridCellXZ<UnitValue>> GetAvaiableCells()
        {
            return GetCells((v, i) => i.AvaiableValidation(v));
        }

        public List<GridCellXZ<UnitValue>> GetAffectedCells(Vector3 position)
        {
            return GetCells((v, i) => i.AffectedValidation(v, position));
        }

        public bool IsCellAvailable(Vector3 pos)
        {
            if(!CurrentIntent.IsPresentAndGet(out IGridIntent intent))
                return false;

            var cell = gridManager.Grid.GetCell(pos);
            var validations = intent.AvaiableValidation(gridManager.Validator()).Build();
            return validations.All(v => v.IsAvailable(cell));
        }

        private List<GridCellXZ<UnitValue>> GetCells(
            Func<GridValidator, IGridIntent, GridValidator> callback
        )
        {
            if(!CurrentIntent.IsPresentAndGet(out IGridIntent intent))
                return new();

            var validator = gridManager.Validator();
            var validations = callback(validator, intent).Build();
            return gridManager.GetCells(validations).ToList();
        }
    }
}
