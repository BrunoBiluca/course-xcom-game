using System;
using Moq;
using NUnit.Framework;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets.Tests
{
    public class SpinUnitActionTests
    {
        [Test]
        public void Should_finish_action_when_spin_is_complete()
        {
            var actor = new Mock<IUnitActor>();

            Action<float> updateCallback = null;
            actor.Setup((a) => a.SetUpdateCallback(It.IsAny<Action<float>>()))
                .Callback<Action<float>>((a) => updateCallback = a);

            var transform = new Mock<ITransform>();
            var spinAction = new SpinUnitAction(actor.Object, transform.Object);

            var wasFinished = false;
            spinAction.OnFinishAction += () => wasFinished = true;
            spinAction.Execute();

            updateCallback(360f);

            Assert.That(wasFinished, Is.True);
        }
    }
}