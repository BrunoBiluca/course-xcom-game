using Moq;
using UnityEngine;
using UnityFoundation.HealthSystem;

namespace GameAssets.Tests
{
    public class CharacterUnitMock
    {
        public Mock<IDamageable> Damageable { get; private set; }

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
            unit.Setup(u => u.Damageable).Returns(Damageable.Object);
            unit.Setup((u) => u.Transform.Position).Returns(startPosition);

            return unit;
        }
    }
}