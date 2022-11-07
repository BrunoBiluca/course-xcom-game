using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitWorldGridXZManager : WorldGridXZManager<GridUnitValue>
    {
        public List<ITransform> Transforms { get; private set; }

        public UnitWorldGridXZManager(IWorldGridXZ<GridUnitValue> worldGrid) : base(worldGrid)
        {
            Transforms = new List<ITransform>();
        }

        public void Add(ITransform transform)
        {
            Transforms.Add(transform);
        }

        public void Update()
        {
            foreach(var gridCell in Grid.Cells)
            {
                gridCell.Clear();
            }

            foreach(var transform in Transforms)
            {
                UpdateGridPosition(transform);
            }
        }

        private void UpdateGridPosition(ITransform transform)
        {
            var newGridPos = Grid.GetCellWorldPosition(transform.Position);
            Grid.TryUpdateValue(newGridPos, (val) => val.Add(transform));
        }
    }
}
