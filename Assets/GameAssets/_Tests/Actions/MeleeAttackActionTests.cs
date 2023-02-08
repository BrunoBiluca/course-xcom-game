using Moq;
using NUnit.Framework;
using UnityFoundation.HealthSystem;

namespace GameAssets.Tests
{
    public class MeleeAttackActionTests
    {
        [Test]
        public void Should_attack_when_is_close_to_target()
        {
            var attacker = new CharacterUnitMock();
            var target = new CharacterUnitMock();

            var actionConfig = new MeleeAttackAction.Settings(2);

            var action = new MeleeAttackAction(
                actionConfig, 
                attacker.Build(), 
                target.Build()
            );

            action.Execute();

            attacker.AnimatorController.Object.AnimationEventHandler(UnitAnimationEvents.MELEE);

            Assert.That(target.WasDamaged, Is.True);
        }
    }
}