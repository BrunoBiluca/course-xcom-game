using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets.Tests
{
    public class UnitWorldGridManagerBuilder
    {
        public List<CharacterUnitMock> Units { get; private set; }

        public UnitWorldGridManager Build()
        {
            Units = new List<CharacterUnitMock>();
            for(int x = 0; x < 3; x++)
                for(int z = 0; z < 3; z++)
                    Units.Add(new CharacterUnitMock()
                        .WithPosition(new Vector3(x, 0, z)));

            var worldGrid = new WorldGridXZ<UnitValue>(
                Vector3.zero, 3, 3, 1, () => new UnitValue()
            );
            var gridManager = new UnitWorldGridManager(worldGrid);

            foreach(var unit in Units)
                gridManager.Add(unit.Build());

            gridManager.Update();

            return gridManager;
        }
    }
}