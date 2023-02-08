using Moq;
using NUnit.Framework;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.CharacterSystem.ActorSystem.Tests;
using UnityFoundation.TestUtility;

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
                () => actionHandler.SetIntent(new Mock<IAPIntent>().Object)
            );
        }

        [Test]
        public void Should_throw_error_when_trying_to_sect_action_with_no_action_points_avaiable()
        {
            var actionHandler = new UnitActionHandlerBuilder()
                .WithCurrentUnitSelected(0)
                .Build();

            Assert.Throws<NoAPAvaiable>(
                () => actionHandler.SetIntent(new Mock<IAPIntent>().Object)
            );

        }

        [Test]
        public void Given_action_is_selected_should_return_an_action()
        {
            var actionHandler = new UnitActionHandlerBuilder().WithCurrentUnitSelected().Build();

            var actionSelected = EventTest<IAPIntent>
                .Create(actionHandler, nameof(actionHandler.OnActionSelected));

            actionHandler.SetIntent(new Mock<IAPIntent>().Object);

            var action = actionHandler.CurrentAction;
            Assert.That(action.IsPresent, Is.True, "should have action");
            Assert.That(
                actionSelected.WasTriggered, Is.True, "should execute on action selected once"
            );
        }

        [Test]
        public void Should_deselect_action_when_action_was_selected()
        {
            var actionHandler = new UnitActionHandlerBuilder().WithCurrentUnitSelected().Build();

            actionHandler.SetIntent(new Mock<IAPIntent>().Object);

            var actionUnselected = EventTest
                .Create(actionHandler, nameof(actionHandler.OnActionUnselected));

            actionHandler.UnselectAction();

            var action = actionHandler.CurrentAction;
            Assert.That(action.IsPresent, Is.False, "should not have action");
            Assert.That(
                actionUnselected.WasTriggered, Is.True, "should execute on action deselected once"
            );
        }
    }

}