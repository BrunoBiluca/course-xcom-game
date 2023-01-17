using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets.Tests
{
    public partial class UnitSelectionTests
    {
        [Test]
        public void Should_not_select_any_unit_if_found_none()
        {
            var raycastHandler = new RaycastHandler(new Mock<ICamera>().Object);
            var unitSelection = new RaycastSelector(raycastHandler);

            var unit = unitSelection.Select(Vector3.zero);

            Assert.IsFalse(unit.IsPresent);
        }

        [Test]
        public void Should_select_unit_if_found_on_coordenate()
        {
            var testCase = new TestCase();
            testCase.FoundUnit();
            var unitSelection = new RaycastSelector(testCase.GetRaycastHandler());

            var selectedUnit = unitSelection.Select(Vector3.zero);

            Assert.IsTrue(selectedUnit.IsPresent);
        }

        [Test]
        public void Given_unit_was_selected_should_deselect_it_if_found_none()
        {
            var testCase = new TestCase();
            testCase.FoundUnit();

            var unitSelection = new RaycastSelector(testCase.GetRaycastHandler());

            Assert.IsTrue(unitSelection.Select(Vector3.zero).IsPresent);

            testCase.NotFoundUnit();

            unitSelection.Select(Vector3.one);

            Assert.IsFalse(testCase.GetSelectableUnit().State);
            Assert.IsFalse(unitSelection.Select(Vector3.zero).IsPresent);
        }

        [Test]
        public void Given_unit_was_selected_should_return_none_if_unit_is_destroyed()
        {
            var testCase = new TestCase();
            testCase.FoundUnit();

            var unitSelection = new RaycastSelector(testCase.GetRaycastHandler());

            Assert.IsTrue(unitSelection.Select(Vector3.zero).IsPresent);

            testCase.DestroyUnit();

            Assert.IsFalse(unitSelection.CurrentUnit);
        }

    }
}