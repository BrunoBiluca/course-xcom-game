using Moq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{
    public sealed class UnitWorldGridManagerFakeBuilder : FakeBuilder<IUnitWorldGridManager>
    {
        private readonly List<IUnit> units = new();

        public List<CharacterUnitMock> Units = new();

        public int GridWidth { get; set; } = 3;
        public int GridDepth { get; set; } = 3;

        public UnitWorldGridManagerFakeBuilder AddUnit(UnitFactions faction, Vector3 position)
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
            for(int x = 0; x < GridWidth; x++)
                for(int z = 0; z < GridDepth; z++)
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
            worldGrid.Setup(new GridXZConfig() {
                Width = GridWidth,
                Depth = GridDepth,
                CellSize = 1
            });

            var gridManager = new UnitWorldGridManager(
                worldGrid,
                new Mock<IAsyncProcessor>().Object
            );

            foreach(var u in units)
                gridManager.Add(u);

            gridManager.Update();

            return gridManager;
        }

        public UnitWorldGridManagerFakeBuilder WithInteractableUnit()
        {
            var mock = new Mock<IInteractableUnit>();
            var transform = new Mock<ITransform>();
            transform.Setup(t => t.IsValid).Returns(true);
            mock.Setup(m => m.Transform).Returns(transform.Object);
            units.Add(mock.Object);
            return this;
        }
    }
}