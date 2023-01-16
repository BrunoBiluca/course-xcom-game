using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnityEngine;

namespace GameAssets.Tests
{

    public sealed class EnemyBrainTests
    {
        [Test]
        public void Should_not_execute_any_action_given_unit_has_no_action_points()
        {
            var gridManagerMock = new UnitWorldGridManagerMockBuilder();
            var enemyMock = new AIUnitMockBuilder();
            var intentsFactory = new Mock<IEnemyActionIntentFactory>();

            var enemyBrain = new EnemyBrain(
                enemyMock.Build(),
                intentsFactory.Object,
                gridManagerMock.Build()
            );

            enemyBrain.Evaluate();

            enemyMock.OnActionFinishedWas(false);
        }

        [Test]
        public void Should_shoot_when_unit_is_in_shooting_range()
        {
            var targetPosition = Vector3.one;
            var gridManagerMock = new UnitWorldGridManagerMockBuilder()
                .WithUnit(UnitFactions.Player, targetPosition);
            var enemyMock = new AIUnitMockBuilder() { InitialAP = 1 };
            var factoryMockBuilder = new EnemyIntentFactoryMockBuilder();

            var enemyUnit = enemyMock.Build();
            var enemyBrain = new EnemyBrain(
                enemyUnit,
                factoryMockBuilder.Build(),
                gridManagerMock.Build()
            );

            var intent = enemyBrain.ChooseIntent();

            Assert.That(intent, Is.Not.Null);

            factoryMockBuilder.Mock.Verify(
                f => f.IntentShoot(
                    It.Is<ICharacterUnit>(u => u == enemyUnit),
                    It.Is<Vector3>(p => p == targetPosition)
                ),
                Times.Once()
            );
        }

        [Test]
        public void Should_move_when_cant_shoot()
        {
            var targetPosition = new Vector3(2, 0, 2);

            var gridManagerMock = new UnitWorldGridManagerMockBuilder()
                .WithUnit(UnitFactions.Player, targetPosition);

            var enemyMock = new AIUnitMockBuilder() { InitialAP = 1 };

            var factoryMockBuilder = new EnemyIntentFactoryMockBuilder();

            var unit = enemyMock.Build();
            var enemyBrain = new EnemyBrain(
                unit,
                factoryMockBuilder.Build(),
                gridManagerMock.Build()
            );

            var intent = enemyBrain.ChooseIntent();

            Assert.That(intent, Is.Not.Null);

            factoryMockBuilder.Mock.Verify(
                f => f.IntentMove(
                    It.Is<ICharacterUnit>(u => u == unit),
                    It.Is<Vector3>(p => p != unit.Transform.Position)
                ),
                Times.Once()
            );
        }

        [Test]
        public void Should_move_to_an_available_place()
        {
            var targetPosition = new Vector3(2, 0, 0);
            var enemyPosition = new Vector3(1, 0, 0);
            var initialPosition = Vector3.zero;

            var gridManagerMock = new UnitWorldGridManagerMockBuilder()
                .WithUnit(UnitFactions.Player, targetPosition)
                .WithUnit(UnitFactions.Enemy, enemyPosition);

            var enemyMock = new AIUnitMockBuilder() { InitialAP = 1 };

            var factoryMockBuilder = new EnemyIntentFactoryMockBuilder();

            var unit = enemyMock.Build();
            var gridManager = gridManagerMock.Build();
            var enemyBrain = new EnemyBrain(
                unit,
                factoryMockBuilder.Build(),
                gridManager
            );

            var intent = enemyBrain.ChooseIntent();

            Assert.That(intent, Is.Not.Null);

            factoryMockBuilder.Mock.Verify(
                f => f.IntentMove(
                    It.Is<ICharacterUnit>(u => u == unit),
                    It.Is<Vector3>(p => gridManager.Grid.GetCell(p).IsEmpty())
                ),
                Times.Once()
            );
        }
    }
}