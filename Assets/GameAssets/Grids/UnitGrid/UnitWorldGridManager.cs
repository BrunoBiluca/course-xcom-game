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
    public class UnitWorldGridManager : WorldGridXZManager<UnitValue>
    {
        public List<IUnit> Units { get; private set; }

        public UnitWorldGridManager(IWorldGridXZ<UnitValue> worldGrid) : base(worldGrid)
        {
            Units = new List<IUnit>();
        }

        public void Add(IUnit unit)
        {
            Units.Add(unit);
        }

        public void Update()
        {
            foreach(var gridCell in Grid.Cells)
            {
                gridCell.Clear();
            }

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
    }
}
