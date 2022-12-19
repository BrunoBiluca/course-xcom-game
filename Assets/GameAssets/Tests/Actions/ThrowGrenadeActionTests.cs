using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{

    public class ThrowGrenadeActionTests : MonoBehaviour
    {
        [Test]
        public void Should_not_execute_when_no_unit_is_in_explosion_range()
        {
            var worldGrid = new WorldGridXZ<UnitValue>(
                Vector3.zero, 3, 3, 1, () => new UnitValue()
            );
            var gridManager = new UnitWorldGridManager(worldGrid);

            var projectileFactory = new Mock<IProjectileFactory>();
            var projectile = new Mock<IProjectile>();
            projectileFactory
                .Setup(pf => pf.Create(It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Returns(projectile.Object);

            var action = new ThrowGrenadeAction(
                gridManager, Vector3.zero, Vector3.one, projectileFactory.Object
            );

            var eventTest = EventTest.Create(action, nameof(action.OnCantExecuteAction));

            action.Execute();

            projectile.Raise(mock => mock.OnReachTarget += null);

            Assert.That(eventTest.WasTriggered, Is.True);
        }

        [Test]
        [TestCaseSource(nameof(BuildGridWithUnits))]
        public void Should_damage_when_unit_is_in_explosion_range(
            UnitWorldGridManagerBuilder gridManagerBuilder,
            Vector3 explosionPosition,
            int explosionRange,
            int expectedDamagedUnits
        )
        {
            var gridManager = gridManagerBuilder.Build();

            var projectileFactory = new Mock<IProjectileFactory>();
            var projectile = new Mock<IProjectile>();
            projectileFactory
                .Setup(pf => pf.Create(It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Returns(projectile.Object);

            var action = new ThrowGrenadeAction(
                gridManager, Vector3.zero, explosionPosition, projectileFactory.Object
            ) {
                Config = new ThrowGrenadeAction.Settings() { ExplosionRange = explosionRange }
            };

            var cantExecuteEvent = EventTest.Create(action, nameof(action.OnCantExecuteAction));
            var finishEvent = EventTest.Create(action, nameof(action.OnFinishAction));

            action.Execute();

            projectile.Raise(mock => mock.OnReachTarget += null);

            Assert.That(cantExecuteEvent.WasTriggered, Is.False);
            Assert.That(finishEvent.WasTriggered, Is.True);

            Assert.That(
                gridManagerBuilder.Units.Where(u => u.WasDamaged).Count(),
                Is.EqualTo(expectedDamagedUnits)
            );
        }

        private static IEnumerable<TestCaseData> BuildGridWithUnits()
        {
            yield return new TestCaseData(new UnitWorldGridManagerBuilder(), Vector3.one, 0, 1)
                .SetName("when unit is in the certer of explosion");

            yield return new TestCaseData(new UnitWorldGridManagerBuilder(), Vector3.one, 1, 9)
                .SetName("when units are in explosion range");

            yield return new TestCaseData(new UnitWorldGridManagerBuilder(), Vector3.zero, 1, 4)
                .SetName("when explosion was in the corner");
        }
    }
}