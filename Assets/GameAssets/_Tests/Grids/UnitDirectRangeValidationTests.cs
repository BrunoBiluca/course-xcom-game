using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets.Tests
{
    public class UnitDirectRangeValidationTests
    {

        [Test]
        public void Should_not_be_available_when_range_is_zero()
        {
            var grid = new GameObject("grid").AddComponent<UnitWorldGridXZ>();
            grid.Config = new GridXZConfig() {
                Width = 2,
                Depth = 2,
                CellSize = 1
            };
            grid.Awake();
            var gridManager = new UnitWorldGridManager(grid, new Mock<IAsyncProcessor>().Object);
            gridManager.Update();

            var validation = new UnitDirectRangeValidation(gridManager, grid.Cells[0, 0], 0);

            Assert.That(validation.IsAvailable(grid.Cells[1, 0]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[0, 1]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[1, 1]), Is.False);
        }

        [Test]
        public void Should_be_available_when_range_is_one()
        {
            var grid = new GameObject("grid").AddComponent<UnitWorldGridXZ>();
            grid.Config = new GridXZConfig() {
                Width = 2,
                Depth = 2,
                CellSize = 1
            };
            grid.Awake();
            var gridManager = new UnitWorldGridManager(grid, new Mock<IAsyncProcessor>().Object);
            gridManager.Update();

            var validation = new UnitDirectRangeValidation(gridManager, grid.Cells[0, 0], 1);

            Assert.That(validation.IsAvailable(grid.Cells[1, 0]), Is.True);
            Assert.That(validation.IsAvailable(grid.Cells[0, 1]), Is.True);
            Assert.That(validation.IsAvailable(grid.Cells[1, 1]), Is.False);
        }

        [Test]
        public void Should_not_be_available_when_has_object_blocking_path()
        {
            var grid = new GameObject("grid").AddComponent<UnitWorldGridXZ>();
            grid.Config = new GridXZConfig() {
                Width = 3,
                Depth = 3,
                CellSize = 1
            };
            grid.Awake();

            var gridManager = new UnitWorldGridManager(grid, new Mock<IAsyncProcessor>().Object);
            gridManager.Add(MockBlockingUnit(new Vector3(1, 0, 0)));
            gridManager.Add(MockBlockingUnit(new Vector3(1, 0, 1)));
            gridManager.Add(MockBlockingUnit(new Vector3(0, 0, 1)));
            gridManager.Update();

            var validation = new UnitDirectRangeValidation(gridManager, grid.Cells[0, 0], 3);

            Assert.That(validation.IsAvailable(grid.Cells[1, 0]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[0, 1]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[1, 1]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[2, 0]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[0, 2]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[2, 2]), Is.False);
        }

        private IUnit MockBlockingUnit(Vector3 position)
        {
            var blocking = new Mock<IUnit>();
            blocking.Setup(b => b.IsBlockable).Returns(true);

            var transform = new Mock<ITransform>();
            transform.Setup(t => t.Position).Returns(position);
            transform.Setup(t => t.IsValid).Returns(true);
            blocking.Setup(b => b.Transform).Returns(transform.Object);

            return blocking.Object;
        }
    }
}