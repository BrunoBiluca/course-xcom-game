using Moq;
using System;
using UnityEngine;
using UnityFoundation.HealthSystem;

namespace GameAssets.Tests
{
    public class CharacterUnitMock
    {
        public Mock<IDamageable> Damageable { get; private set; }
        public Mock<IAnimatorController> AnimatorController { get; private set; }

        public bool WasDamaged { get; private set; } = false;

        private Vector3 startPosition;

        public CharacterUnitMock WithPosition(Vector3 position)
        {
            startPosition = position;
            return this;
        }

        public Mock<ICharacterUnit> Build()
        {
            Damageable ??= new Mock<IDamageable>();
            Damageable
                .Setup(d => d.Damage(It.IsAny<float>(), null))
                .Callback(() => WasDamaged = true);

            var unit = new Mock<ICharacterUnit>();

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

        public ICharacterUnit Object()
        {
            return Build().Object;
        }
    }
}