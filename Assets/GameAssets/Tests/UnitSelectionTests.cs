using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets.Tests
{
    public class UnitSelectionTests
    {
        [Test]
        public void Should_not_select_any_unit_if_found_none()
        {
            var raycastHandler = new RaycastHandler(new Mock<ICamera>().Object);
            var unitSelection = new UnitSelection(raycastHandler);

            var unit = unitSelection.Select(Vector3.zero);

            Assert.IsFalse(unit.IsPresent);
        }

        [Test]
        public void Should_select_unit_if_found_on_coordenate()
        {
            var unit = new GameObject("unit").AddComponent<UnitMono>();
            unit.gameObject.AddComponent<BoxCollider>();

            var raycastHandler = new Mock<IRaycastHandler>();
            raycastHandler
                .Setup(rh => rh.GetObjectOf<ISelectable>(Vector2.zero, 0))
                .Returns(unit);

            var unitSelection = new UnitSelection(raycastHandler.Object);

            var selectedUnit = unitSelection.Select(unit.transform.position);

            Assert.IsTrue(selectedUnit.IsPresent);
        }
    }
}