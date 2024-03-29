using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFoundation.Code.Grid;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{

    public class ThrowGrenadeActionTests : MonoBehaviour
    {
        [Test]
        [TestCaseSource(nameof(BuildGridWithUnits))]
        public void Should_damage_when_unit_is_in_explosion_range(
            UnitWorldGridManagerFakeBuilder gridManagerBuilder,
            Vector3 explosionPosition,
            int explosionRange,
            int expectedDamagedUnits
        )
        {
            var gridManager = gridManagerBuilder.FilledWithUnits().Build();

            var projectileFactoryMock = new ProjectileFactoryMock();
            projectileFactoryMock.Build();

            var action = new ThrowGrenadeAction(
                new AreaAttackSettings() { ExplosionRange = explosionRange },
                gridManager,
                Vector3.zero,
                explosionPosition,
                projectileFactoryMock.ProjectileFactory
            );

            var cantExecuteEvent = EventTest.Create(action, nameof(action.OnCantExecuteAction));
            var finishEvent = EventTest.Create(action, nameof(action.OnFinishAction));

            action.Execute();

            projectileFactoryMock.RaiseProjectileReachedTarget();

            Assert.That(cantExecuteEvent.WasTriggered, Is.False);
            Assert.That(finishEvent.WasTriggered, Is.True);

            Assert.That(
                gridManagerBuilder.Units.Where(u => u.WasDamaged).Count(),
                Is.EqualTo(expectedDamagedUnits)
            );
        }

        private static IEnumerable<TestCaseData> BuildGridWithUnits()
        {
            yield return new TestCaseData(new UnitWorldGridManagerFakeBuilder(), Vector3.one, 0, 1)
                .SetName("when unit is in the certer of explosion");

            yield return new TestCaseData(new UnitWorldGridManagerFakeBuilder(), Vector3.one, 1, 5)
                .SetName("when units are in explosion range");

            yield return new TestCaseData(new UnitWorldGridManagerFakeBuilder(), Vector3.zero, 1, 3)
                .SetName("when explosion was in the corner");
        }
    }
}