using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityFoundation.Code;
using System.Linq;
using UnityFoundation.Code.Grid;

namespace GameAssets.Tests
{
    public class UnitWorldGridManagerTests
    {
        [Test]
        public void Should_return_units_given_X_range()
        {
            var gridManager = new UnitWorldGridManagerFakeBuilder()
                .FilledWithUnits()
                .Build();

            var unitsLeftBottom = gridManager.GetUnitsInRange(Vector3.zero, 1);
            Assert.That(unitsLeftBottom.Count, Is.EqualTo(3));

            var unitsCenter = gridManager.GetUnitsInRange(Vector3.one, 1);
            Assert.That(unitsCenter.Count, Is.EqualTo(5));

            var unitsRightTop = gridManager.GetUnitsInRange(new Vector3(2, 2), 1);
            Assert.That(unitsRightTop.Count, Is.EqualTo(3));
        }
    }
}