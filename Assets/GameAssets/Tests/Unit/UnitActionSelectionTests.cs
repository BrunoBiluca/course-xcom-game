using Moq;
using NUnit.Framework;
using System;

namespace GameAssets.Tests
{
    public sealed partial class UnitActionSelectionTests
    {

        [Test]
        public void Should_return_no_action_when_action_is_not_selected()
        {
            var actionHandler = new UnitActionHandlerBuilder().Build();

            var action = actionHandler.CurrentAction;
            Assert.That(action.IsPresent, Is.False);
        }

        [Test]
        public void Should_throw_error_when_trying_to_set_action_to_deselected_unit()
        {
            var actionHandler = new UnitActionHandlerBuilder().Build();

            Assert.Throws<ActorIsNotSelected>(
                () => actionHandler.SetAction(new Mock<IUnitAction>().Object)
            );
        }

        [Test]
        public void Given_action_is_selected_should_return_an_action()
        {
            var actionHandler = new UnitActionHandlerBuilder().WithCurrentUnitSelected().Build();

            var onActionSelectedWasCalled = false;
            actionHandler.OnActionSelected += (_) => onActionSelectedWasCalled = true;

            actionHandler.SetAction(new Mock<IUnitAction>().Object);

            var action = actionHandler.CurrentAction;
            Assert.That(action.IsPresent, Is.True, "should have action");
            Assert.That(
                onActionSelectedWasCalled, Is.True, "should execute on action selected once"
            );
        }

        [Test]
        public void Should_deselect_action_when_action_was_selected()
        {
            var actionHandler = new UnitActionHandlerBuilder().WithCurrentUnitSelected().Build();

            actionHandler.SetAction(new Mock<IUnitAction>().Object);

            var eventWasCalled = false;
            actionHandler.OnActionDeselected += () => eventWasCalled = true;

            actionHandler.DeselectAction();

            var action = actionHandler.CurrentAction;
            Assert.That(action.IsPresent, Is.False, "should not have action");
            Assert.That(
                eventWasCalled, Is.True, "should execute on action deselected once"
            );
        }
    }

}