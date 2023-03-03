using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.TextCore.Text;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.Math;
using static UnityEditor.PlayerSettings;
using Object = UnityEngine.Object;

namespace GameAssets
{
    // TODO: essa classe deve ser severamente refatorada
    // separar as responsabilidades de valida??o do gerenciamento do grid
    public class UnitWorldGridManager : WorldGridManager<UnitValue>, IUnitWorldGridManager
    {
        public List<IUnit> Units { get; private set; }

        public GridPathFinding GridPathFinding { get; private set; }

        public UnitWorldGridManager(
            UnitWorldGridXZ worldGrid, IAsyncProcessor updateProcessor)
            : base(worldGrid.Grid)
        {
            Units = new List<IUnit>();

            updateProcessor.ExecuteEveryFrame(Update);

            GridPathFinding = new GridPathFinding(worldGrid.Grid);
        }

        public void Add(IUnit unit)
        {
            if(Units.Contains(unit))
                return;

            Units.Add(unit);
        }

        public void Update()
        {
            foreach(var gridCell in Grid.Cells)
            {
                gridCell.Clear();
            }

            Units.RemoveAll(u => u == null);
            Units.RemoveAll(u => !u.Transform.IsValid);
            Units.RemoveAll(u => u is ICharacterUnit c && c.HealthSystem.IsDead);

            foreach(var transform in Units)
            {
                UpdateGridPosition(transform);
            }

            GridPathFinding.BuildPathFindingGrid();
        }

        private void UpdateGridPosition(IUnit unit)
        {
            var newGridPos = Grid.GetCellWorldPosition(unit.Transform.Position);
            Grid.TryUpdateValue(newGridPos, (val) => val.Add(unit));
        }

        public GridValidator Validator()
        {
            return new GridValidator(this);
        }

        public List<IUnit> GetUnitsInRange(Vector3 center, int range)
        {
            var units = new List<IUnit>();

            foreach(var position in center.PositionsInRange(range, Grid.CellSize))
            {
                try
                {
                    var value = Grid.GetValue(position);
                    units.AddRange(value.Units);
                }
                catch(ArgumentOutOfRangeException) { }
            }

            return units;
        }

        public IEnumerable<Vector3> GetAllAvailableCellsPositions()
        {
            foreach(var c in GetAllAvailableCells())
            {
                yield return new Vector3(c.Position.X, 0, c.Position.Z) * Grid.CellSize;
            }
        }

        public override IEnumerable<GridCellXZ<UnitValue>> GetCells(
            IGridValidation<UnitValue>[] validations
        )
        {
            return base.GetCells(validations);
        }

        public IEnumerable<Vector3> GetCellsPositions(IGridValidation<UnitValue>[] validations)
        {
            foreach(var c in base.GetCells(validations))
            {
                yield return new Vector3(c.Position.X, 0, c.Position.Z) * Grid.CellSize;
            }
        }

        public void InitUnits()
        {
            foreach(var unit in Object.FindObjectsOfType<MonoBehaviour>().OfType<IUnit>())
            {
                Add(unit);
            }
        }
    }
}
