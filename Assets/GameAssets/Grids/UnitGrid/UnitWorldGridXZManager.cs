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
    public class UnitWorldGridXZManager : WorldGridXZManager<UnitValue>
    {
        public List<IUnit> Units { get; private set; }

        public UnitWorldGridXZManager(IWorldGridXZ<UnitValue> worldGrid) : base(worldGrid)
        {
            Units = new List<IUnit>();
        }

        public void Add(IUnit transform)
        {
            Units.Add(transform);
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

        private void UpdateGridPosition(IUnit transform)
        {
            var newGridPos = Grid.GetCellWorldPosition(transform.Transform.Position);
            Grid.TryUpdateValue(newGridPos, (val) => val.Add(transform));
        }

        public UnitWorldGridValidator Validator()
        {
            return new UnitWorldGridValidator(this);
        }
    }
}
