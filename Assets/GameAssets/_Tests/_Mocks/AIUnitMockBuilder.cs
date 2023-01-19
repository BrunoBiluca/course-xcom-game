using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.ResourceManagement;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{
    public class AIUnitMockBuilder : MockBuilder<IAIUnit>
    {
        public UnitConfig UnitConfig { get; set; }
        public uint InitialAP { get; set; } = 0;
        public Vector3 Position { get; set; } = Vector3.zero;

        private EventTest takeActionEvent;

        public AIUnitMockBuilder()
        {
            UnitConfig = new UnitConfig(null, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        public void OnActionFinishedWas(bool state)
        {
            Assert.That(takeActionEvent.WasTriggered, Is.EqualTo(state));
        }

        public void OnActionFinishedWas(int calledTimes)
        {
            Assert.That(takeActionEvent.TriggerCount, Is.EqualTo(calledTimes));
        }

        protected override Mock<IAIUnit> OnBuild()
        {
            var actor = new APActor(new FiniteResourceManager(InitialAP, true));

            takeActionEvent = new EventTest(
                actor, nameof(actor.OnActionFinished)
            );
            AddToObjects(actor);

            var enemy = new Mock<IAIUnit>();
            enemy.Setup(e => e.UnitConfig).Returns(UnitConfig);
            enemy.Setup(e => e.Actor).Returns(actor);
            enemy.Setup((u) => u.Transform.Position).Returns(Position);
            enemy.Setup((u) => u.Transform.IsValid).Returns(true);

            return enemy;
        }
    }
}