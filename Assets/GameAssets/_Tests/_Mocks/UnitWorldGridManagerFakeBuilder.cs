using Moq;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{
    public sealed class UnitWorldGridManagerFakeBuilder : FakeBuilder<IUnitWorldGridManager>
    {
        private readonly List<IUnit> units = new();

        public List<CharacterUnitMock> Units = new();

        public UnitWorldGridManagerFakeBuilder WithUnit(UnitFactions faction, Vector3 position)
        {
            units.Add(new CharacterUnitMock()
                .WithPosition(position)
                .WithFaction(faction)
                .Build()
            );
            return this;
        }
        public UnitWorldGridManagerFakeBuilder WithSelectableUnit(
            UnitFactions faction,
            Vector3 position
        )
        {
            units.Add(new CharacterUnitMock()
                .Selectable()
                .WithPosition(position)
                .WithFaction(faction)
                .Build()
            );
            return this;
        }

        public UnitWorldGridManagerFakeBuilder FilledWithUnits()
        {
            Units = new();
            for(int x = 0; x < 3; x++)
                for(int z = 0; z < 3; z++)
                {
                    var mock = new CharacterUnitMock().WithPosition(new Vector3(x, 0, z));
                    Units.Add(mock);
                    units.Add(mock.Build());
                }

            return this;
        }

        protected override IUnitWorldGridManager OnBuild()
        {
            var worldGrid = new GameObject("unit_world_grid").AddComponent<UnitWorldGridXZ>();
            worldGrid.Setup(new GridXZConfig() { Width = 3, Depth = 3, CellSize = 1 });

            var gridManager = new UnitWorldGridManager(
                worldGrid,
                new Mock<IAsyncProcessor>().Object
            );

            foreach(var u in units)
                gridManager.Add(u);

            gridManager.Update();

            return gridManager;
        }
    }
}