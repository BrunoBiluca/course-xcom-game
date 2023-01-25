using Moq;
using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.HealthSystem;
using UnityFoundation.SettingsSystem;
using UnityFoundation.TestUtility;

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

        public CharacterUnitMock Selectable()
        {
            unit.Setup(u => u.Selectable).Returns(new Mock<ISelectable>().Object);
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

            unit.Setup(u => u.IsBlockable).Returns(true);
            unit.Setup(u => u.Damageable).Returns(Damageable.Object);
            unit.Setup((u) => u.Transform.Position).Returns(startPosition);
            unit.Setup((u) => u.Transform.IsValid).Returns(true);

            unit
                .Setup(u => u.SoundEffectsController)
                .Returns(new Mock<ISoundEffectsController>().Object);

            unit.Setup(u => u.SoundEffects).Returns(new SoundEffects());

            return unit;
        }
    }
}