using Moq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets.Tests
{
    public class UnitDirectRangeValidationTests
    {

        [Test]
        public void Should_not_be_available_when_range_is_zero()
        {
            var grid = new GridXZ<UnitValue>(2, 2, 1, () => new UnitValue());

            var validation = new UnitDirectRangeValidation(grid, grid.Cells[0, 0], 0);

            Assert.That(validation.IsAvailable(grid.Cells[1, 0]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[0, 1]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[1, 1]), Is.False);
        }

        [Test]
        public void Should_be_available_when_range_is_one()
        {
            var grid = new GridXZ<UnitValue>(2, 2, 1, () => new UnitValue());

            var validation = new UnitDirectRangeValidation(grid, grid.Cells[0, 0], 1);

            Assert.That(validation.IsAvailable(grid.Cells[1, 0]), Is.True);
            Assert.That(validation.IsAvailable(grid.Cells[0, 1]), Is.True);
            Assert.That(validation.IsAvailable(grid.Cells[1, 1]), Is.False);
        }

        [Test]
        public void Should_not_be_available_when_has_object_blocking_path()
        {
            var blocking = new Mock<IUnit>();
            blocking.Setup(b => b.IsBlockable).Returns(true);
            blocking.Setup(b => b.Transform).Returns(new Mock<ITransform>().Object);

            var grid = new GridXZ<UnitValue>(3, 3, 1, () => new UnitValue());
            grid.TryUpdateValue(new GridCellPositionScaledXZ(1, 0), v => v.Add(blocking.Object));
            grid.TryUpdateValue(new GridCellPositionScaledXZ(1, 1), v => v.Add(blocking.Object));
            grid.TryUpdateValue(new GridCellPositionScaledXZ(0, 1), v => v.Add(blocking.Object));

            var validation = new UnitDirectRangeValidation(grid, grid.Cells[0, 0], 3);

            Assert.That(validation.IsAvailable(grid.Cells[1, 0]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[0, 1]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[1, 1]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[2, 0]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[0, 2]), Is.False);
            Assert.That(validation.IsAvailable(grid.Cells[2, 2]), Is.False);
        }
    }
}