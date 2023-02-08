using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    // TODO: essa classe deve ser severamente refatorada
    // separar as responsabilidades de valida??o do gerenciamento do grid
    public class UnitWorldGridManager : WorldGridManager<UnitValue>, IUnitWorldGridManager
    {
        public enum GridState
        {
            Movement,
            Attack,
            Interact,
            None
        }

        public List<IUnit> Units { get; private set; }

        public GridState State { get; set; }

        public UnitWorldGridManager(
            UnitWorldGridXZ worldGrid, IAsyncProcessor updateProcessor)
            : base(worldGrid.Grid)
        {
            Units = new List<IUnit>();
            State = GridState.None;

            updateProcessor.ExecuteEveryFrame(Update);
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

            foreach(var transform in Units)
            {
                UpdateGridPosition(transform);
            }
        }

        private void UpdateGridPosition(IUnit unit)
        {
            var newGridPos = Grid.GetCellWorldPosition(unit.Transform.Position);
            Grid.TryUpdateValue(newGridPos, (val) => val.Add(unit));
        }

        public UnitWorldGridValidator Validator()
        {
            return new UnitWorldGridValidator(this);
        }

        public List<IUnit> GetUnitsInRange(Vector3 center, int range)
        {
            var units = new List<IUnit>();

            var xMin = (int)(center.x - range * Grid.CellSize);
            var xMax = (int)(center.x + range * Grid.CellSize);

            var zMin = (int)(center.z - range * Grid.CellSize);
            var zMax = (int)(center.z + range * Grid.CellSize);

            for(int x = xMin; x <= xMax; x += Grid.CellSize)
            {
                for(int z = zMin; z <= zMax; z += Grid.CellSize)
                {
                    try
                    {
                        var unitValue = Grid.GetValue(new Vector3(x, 0, z));
                        units.AddRange(unitValue.Units);
                    }
                    catch(ArgumentOutOfRangeException) { }
                }
            }

            return units;
        }

        public override void ResetValidation()
        {
            base.ResetValidation();
            State = GridState.None;
        }

        public IEnumerable<Vector3> GetAllAvailableCellsPositions()
        {
            foreach(var c in GetAllAvailableCells())
            {
                yield return new Vector3(c.Position.X, 0, c.Position.Z) * Grid.CellSize;
            }
        }
    }


}
