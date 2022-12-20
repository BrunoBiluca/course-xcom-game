using Assets.UnityFoundation.Systems.HealthSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitWorldGridManager : WorldGridManager<UnitValue>
    {
        public enum GridState
        {
            None,
            Attack,
            Interact
        }

        public List<IUnit> Units { get; private set; }

        public GridState State { get; set; }

        public UnitWorldGridManager(IWorldGridXZ<UnitValue> worldGrid) : base(worldGrid)
        {
            Units = new List<IUnit>();
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
    }
}
