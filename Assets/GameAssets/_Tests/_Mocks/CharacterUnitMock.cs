using Moq;
using System;
using UnityEngine;
using UnityFoundation.HealthSystem;

namespace GameAssets.Tests
{
    public class CharacterUnitMock : MockBuilder<ICharacterUnit>
    {
        public Mock<IDamageable> Damageable { get; private set; }
        public Mock<IAnimatorController> AnimatorController { get; private set; }

        public bool WasDamaged { get; private set; } = false;

        private Vector3 startPosition;

        private Mock<ICharacterUnit> unit;

        public CharacterUnitMock()
        {
            unit = new Mock<ICharacterUnit>();
        }

        public CharacterUnitMock WithPosition(Vector3 position)
        {
            startPosition = position;
            return this;
        }

        public CharacterUnitMock WithFaction(UnitFactions faction)
        {
            unit.Setup(u => u.Faction).Returns(faction);
            return this;
        }

        protected override Mock<ICharacterUnit> OnBuild()
        {
            Damageable ??= new Mock<IDamageable>();
            Damageable
                .Setup(d => d.Damage(It.IsAny<float>(), null))
                .Callback(() => WasDamaged = true);

            AnimatorController ??= new Mock<IAnimatorController>();
            AnimatorController
                .Setup(a => a.AnimationEventHandler(It.IsAny<string>()))
                .Callback<string>(name => {
                    AnimatorController.Raise(mock => mock.OnEventTriggered += null, name);
                });
            unit.Setup(u => u.AnimatorController).Returns(AnimatorController.Object);

            unit.Setup(u => u.Damageable).Returns(Damageable.Object);
            unit.Setup((u) => u.Transform.Position).Returns(startPosition);
            unit.Setup((u) => u.Transform.IsValid).Returns(true);

            return unit;
        }
    }
}