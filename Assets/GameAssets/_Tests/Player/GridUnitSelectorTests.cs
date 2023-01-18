using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets.Tests
{
    public class GridUnitSelectorTests : MonoBehaviour
    {
        [Test]
        public void Should_not_select_any_unit_when_grid_is_empty()
        {
            var gridManager = new UnitWorldGridManagerMockBuilder().Build();
            var selector = new GridSelector(gridManager);

            var selected = selector.Select(Vector3.zero);

            Assert.That(selected.IsPresent, Is.False);
            Assert.That(selector.CurrentUnit.IsPresent, Is.False);
        }

        [Test]
        public void Should_not_select_unit_when_unit_is_in_another_cell_position()
        {
            var gridManager = new UnitWorldGridManagerMockBuilder()
                .WithUnit(UnitFactions.Player, Vector3.forward)
                .Build();
            var selector = new GridSelector(gridManager);

            var selected = selector.Select(Vector3.zero);

            Assert.That(selected.IsPresent, Is.False);
            Assert.That(selector.CurrentUnit.IsPresent, Is.False);
        }

        [Test]
        public void Should_select_unit_when_it_is_in_cell_position()
        {
            var gridManager = new UnitWorldGridManagerMockBuilder()
                .WithSelectableUnit(UnitFactions.Player, Vector3.forward)
                .Build();
            var selector = new GridSelector(gridManager);

            var selected = selector.Select(Vector3.forward);

            Assert.That(selected.IsPresent, Is.True);
            Assert.That(selector.CurrentUnit.IsPresent, Is.True);
        }

    }
}