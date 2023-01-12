﻿using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets.Tests
{
    public sealed class UnitWorldGridManagerMockBuilder : BilucaBuilder<IUnitWorldGridManager>
    {
        private readonly List<IUnit> units = new();

        public UnitWorldGridManagerMockBuilder WithUnit(UnitFactions faction, Vector3 position)
        {
            units.Add(new CharacterUnitMock()
                .WithPosition(position)
                .WithFaction(faction)
                .Build()
            );
            return this;
        }

        protected override IUnitWorldGridManager OnBuild()
        {
            var grid = new WorldGridXZ<UnitValue>(Vector3.zero, 3, 3, 1, () => new UnitValue());
            var gridManager = new UnitWorldGridManager(grid);

            foreach(var u in units)
                gridManager.Add(u);

            gridManager.Update();

            return gridManager;
        }
    }
}