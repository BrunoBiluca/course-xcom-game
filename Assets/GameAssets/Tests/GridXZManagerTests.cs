using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;

namespace GameAssets.Tests
{
    public class GridXZManagerTests
    {
        class TestGridValue : IEmptyable
        {
            public string text;

            public bool IsEmpty()
            {
                return text == null;
            }
        }

        [Test]
        public void Should_return_all_cells_when_grid_was_only_initialized()
        {
            var grid = new WorldGridXZ<string>(Vector3.zero, 2, 2, 1);
            var gridManager = new WorldGridXZManager<string>(grid);

            var cells = gridManager.GetAllValidCells().Count();

            Assert.That(cells, Is.EqualTo(4));
        }

        [Test]
        public void Should_return_nothing_when_grid_was_all_filled()
        {
            var grid = new WorldGridXZ<string>(Vector3.zero, 2, 2, 1);
            grid.Fill("filled");

            var gridManager = new WorldGridXZManager<string>(grid);
            var cells = gridManager.GetAllValidCells().Count();

            Assert.That(cells, Is.EqualTo(0));
        }

        [Test]
        public void Should_return_some_cells_when_values_are_set()
        {
            var grid = new WorldGridXZ<TestGridValue>(Vector3.zero, 2, 2, 1);
            var gridManager = new WorldGridXZManager<TestGridValue>(grid);

            grid.TrySetValue(Vector3.zero, new TestGridValue() { text = "zero" });
            grid.TrySetValue(Vector3.one, new TestGridValue() { text = "one" });

            var cells = gridManager.GetAllValidCells().Count();

            Assert.That(cells, Is.EqualTo(2));
        }

        [Test]
        public void Should_return_some_cells_when_values_are_updated()
        {
            var grid = new WorldGridXZ<TestGridValue>(Vector3.zero, 2, 2, 1);
            var gridManager = new WorldGridXZManager<TestGridValue>(grid);

            grid.TrySetValue(Vector3.zero, new TestGridValue());
            grid.TrySetValue(Vector3.one, new TestGridValue());

            Assert.That(gridManager.GetAllValidCells().Count(), Is.EqualTo(4));

            grid.TryUpdateValue(Vector3.zero, (value) => value.text = "zero");
            grid.TryUpdateValue(Vector3.one, (value) => value.text = "one");

            Assert.That(gridManager.GetAllValidCells().Count(), Is.EqualTo(2));
        }
    }
}