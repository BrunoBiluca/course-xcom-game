using Moq;
using UnityFoundation.ResourceManagement;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{
    public partial class UnitActionsManagerTests
    {
        public class ContextBuilder
        {
            public Mock<IUnitAction> MockAction { get; private set; }

            public EventTest CantExecuteAction { get; private set; }

            IResourceManager actionPoints;
            private bool isActionSet;
            private int actionPointsCost = 1;

            public ContextBuilder()
            {
                isActionSet = false;
                MockAction = new Mock<IUnitAction>();
                actionPoints = new FiniteResourceManager(0, true);
            }

            public ContextBuilder WithActionSet(int actionPointsCost = 1)
            {
                isActionSet = true;
                this.actionPointsCost = actionPointsCost;
                return this;
            }

            public ContextBuilder WithImmediateAction()
            {
                isActionSet = true;
                MockAction = new Mock<IUnitAction>();
                MockAction.SetupGet(a => a.ExecuteImmediatly).Returns(true);
                return this;
            }

            public ContextBuilder WithInitialActionPoints(int amount)
            {
                actionPoints = new FiniteResourceManager((uint)amount, true);
                return this;
            }

            public APUnitActor Build()
            {
                var actionsManager = new APUnitActor(actionPoints);

                if(isActionSet)
                {
                    var action = new APUnitAction(MockAction.Object) {
                        ActionPointsCost = actionPointsCost
                    };
                    actionsManager.Set(action);
                }


                CantExecuteAction = EventTest
                    .Create(actionsManager, nameof(actionsManager.OnCantExecuteAction));

                return actionsManager;
            }
        }
    }
}