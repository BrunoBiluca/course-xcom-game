using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.WorldCursors;
using static GameAssets.UnitWorldGridManager;

namespace GameAssets.Tests
{
    public class UnitActionsFactoryTests
    {
        IDependencyContainer container;

        const int SPIN_COST = 1;
        const int MOVE_COST = 2;
        const int MELEE_COST = 3;
        const int GRENADE_COST = 4;
        const int INTERACT_COST = 5;
        const int SHOOT_COST = 6;

        [SetUp]
        public void Setup()
        {
            var binder = new DependencyBinder();

            var actionsConfig = new ActionsConfig();
            actionsConfig.AddCost(UnitActionsEnum.MOVE, MOVE_COST);
            actionsConfig.AddCost(UnitActionsEnum.SPIN, SPIN_COST);
            actionsConfig.AddCost(UnitActionsEnum.GRENADE, GRENADE_COST);
            actionsConfig.AddCost(UnitActionsEnum.METEOR, GRENADE_COST);
            actionsConfig.AddCost(UnitActionsEnum.SHOOT, SHOOT_COST);
            actionsConfig.AddCost(UnitActionsEnum.INTERACT, INTERACT_COST);
            actionsConfig.AddCost(UnitActionsEnum.MELEE, MELEE_COST);

            binder.Register(actionsConfig);

            binder.RegisterModule(new PlayerUnitIntentsModule());
            binder.RegisterModule(new ActionsModule());

            var gridManagerBuilder = new UnitWorldGridManagerFakeBuilder();
            gridManagerBuilder.AddUnit(UnitFactions.Enemy, Vector3.zero).WithInteractableUnit();
            var instance = (UnitWorldGridManager)gridManagerBuilder.Build();
            binder.Register<IUnitWorldGridManager>(instance);
            binder.Register(instance);

            binder.Register(new Mock<IProjectileFactory>().Object, ProjectileFactories.Shoot);
            binder.Register(new Mock<IProjectileFactory>().Object, ProjectileFactories.Meteor);
            binder.Register(new Mock<IProjectileFactory>().Object, ProjectileFactories.Grenade);

            var worldCursor = new Mock<IWorldCursor>();
            worldCursor.Setup(c => c.WorldPosition).Returns(Optional<Vector3>.Some(Vector3.zero));
            binder.Register(worldCursor.Object);

            binder.Register(new Mock<IAsyncProcessor>().Object);

            var character = new Mock<ICharacterUnit>();
            character.Setup(s => s.ProjectileStart).Returns(new Mock<ITransform>().Object);
            character.Setup(s => s.UnitConfig).Returns(new UnitConfig(interactRange: 1));
            character.Setup(s => s.Transform).Returns(new Mock<ITransform>().Object);

            var selectorMock = new Mock<ICharacterSelector>();
            selectorMock.Setup(s => s.CurrentUnit).Returns(character.Object);
            binder.Register(selectorMock.Object);

            container = binder.Build();
        }

        [Test]
        public void Should_return_intent_for_spin_action()
        {
            var factory = container.Resolve<UnitIntentsFactory>();
            var intent = factory.Get(UnitActionsEnum.SPIN);
            AssertIntent(intent, true, SPIN_COST);
        }

        [Test]
        public void Should_return_intent_for_move_action()
        {
            var factory = container.Resolve<UnitIntentsFactory>();
            var intent = factory.Get(UnitActionsEnum.MOVE);
            AssertIntent(intent, false, MOVE_COST);
        }

        [Test]
        public void Should_return_intent_for_melee_action()
        {
            var factory = container.Resolve<UnitIntentsFactory>();
            var intent = factory.Get(UnitActionsEnum.MELEE);
            AssertIntent(intent, false, MELEE_COST);
        }

        [Test]
        public void Should_return_intent_for_shoot_action()
        {
            var factory = container.Resolve<UnitIntentsFactory>();
            var intent = factory.Get(UnitActionsEnum.SHOOT);
            AssertIntent(intent, false, SHOOT_COST);
        }

        [Test]
        public void Should_return_intent_for_meteor_intent()
        {
            var factory = container.Resolve<UnitIntentsFactory>();
            var intent = factory.Get(UnitActionsEnum.METEOR);
            AssertIntent(intent, false, GRENADE_COST);
        }

        [Test]
        public void Should_return_intent_for_interact_action()
        {
            var factory = container.Resolve<UnitIntentsFactory>();
            var intent = factory.Get(UnitActionsEnum.INTERACT);
            AssertIntent(intent, false, INTERACT_COST);
        }

        private void AssertIntent(IGridIntent intent, bool isExecuteImmediatly, int cost)
        {
            Assert.That(intent.ExecuteImmediatly, Is.EqualTo(isExecuteImmediatly));
            Assert.That(intent.ActionPointsCost, Is.EqualTo(cost));

            var action = intent.Create();

            Assert.That(action, Is.Not.Null);
        }
    }
}