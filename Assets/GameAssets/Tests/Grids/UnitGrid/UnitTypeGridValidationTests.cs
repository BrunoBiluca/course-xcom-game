using Moq;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets.Tests
{
    public class UnitTypeGridValidationTests : MonoBehaviour
    {

        [Test]
        public void Should_cell_not_be_available_when_no_units_are_in_it()
        {
            var validation = new UnitTypeGridValidation();

            Assert.That(
                validation.IsAvailable(new GridCellXZ<UnitValue>(0, 0)),
                Is.False
            );
        }

        [Test]
        public void Should_cell_be_available_when_one_unit_is_of_expected_type()
        {
            var validation = new UnitTypeGridValidation();

            var cell = new GridCellXZ<UnitValue>(0, 0) {
                Value = new UnitValue()
            };

            var unit = new Mock<IUnit>();
            unit.SetupGet(u => u.Transform).Returns(new Mock<ITransform>().Object);

            cell.Value.Add(unit.Object);

            Assert.That(validation.IsAvailable(cell), Is.True);
        }

        [Test]
        public void Given_unit_exists_should_cell_be_available_or_not_with_unit_validation()
        {
            var cell = new GridCellXZ<UnitValue>(0, 0) {
                Value = new UnitValue()
            };

            var unit = new Mock<IUnit>();
            unit.SetupGet(u => u.Name).Returns("test_name");
            unit.SetupGet(u => u.Transform).Returns(new Mock<ITransform>().Object);

            cell.Value.Add(unit.Object);

            var validation = new UnitTypeGridValidation(u => u.Name == "test_name");
            Assert.That(validation.IsAvailable(cell), Is.True);

            var validationWrong = new UnitTypeGridValidation(u => u.Name == "other_name");
            Assert.That(validationWrong.IsAvailable(cell), Is.False);

        }
    }
}