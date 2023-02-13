using Moq;
using NUnit.Framework;

namespace GameAssets.Tests
{
    public class InteractActionTests
    {
        [Test]
        public void Should_interact_when_object_was_setup()
        {
            var interactableObject = new Mock<IInteractableUnit>();
            var action = new InteractAction(interactableObject.Object);

            action.Execute();

            interactableObject.Verify(o => o.Interact(), Times.Once());
        }
    }
}