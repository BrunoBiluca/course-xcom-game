using NUnit.Framework;
using System;

namespace GameAssets.Tests
{
    public sealed class UnitActionSelectionTests
    {
        class DummyUnitAction : IUnitAction
        {
            public void Execute()
            {
                // do nothing
            }
        }

        class DummyUnitActionSelector : IUnitActionSelector
        {
            public event Action<IUnitAction> OnActionSelected;

            public void Select()
            {
                OnActionSelected(new SpinUnitAction());
            }
        }

        [Test]
        public void Should_return_no_action_when_action_is_not_selected()
        {
            var unitActionSelection = new UnitActionSelection(new DummyUnitActionSelector());

            var action = unitActionSelection.GetAction();
            Assert.That(action.IsPresent, Is.False);
        }

        [Test]
        public void Given_action_is_selected_should_return_an_action()
        {
            var actionSelector = new DummyUnitActionSelector();
            var unitActionSelection = new UnitActionSelection(actionSelector);

            actionSelector.Select();

            var action = unitActionSelection.GetAction();
            Assert.That(action.IsPresent, Is.True);
            Assert.That(action.Get(), Is.TypeOf<SpinUnitAction>());
        }
    }
}