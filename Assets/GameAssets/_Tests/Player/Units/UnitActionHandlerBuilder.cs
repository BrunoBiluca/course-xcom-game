using GameAssets;
using Moq;
using UnityFoundation.ResourceManagement;

namespace UnityFoundation.CharacterSystem.ActorSystem.Tests
{
    public sealed class UnitActionHandlerBuilder
    {
        private uint initialActionPoints;
        private bool isUnitSelected = false;
        public UnitActionHandlerBuilder WithCurrentUnitSelected(int initialActionPoints = 1)
        {
            this.initialActionPoints = (uint)initialActionPoints;
            isUnitSelected = true;
            return this;
        }
        public IntentSelector Build()
        {
            var unitActorSelector = new Mock<IActorSelector<IAPActor>>();

            if(isUnitSelected)
            {
                var actor = new Mock<IAPActor>();
                actor
                    .Setup(a => a.ActionPoints)
                    .Returns(new FiniteResourceManager(initialActionPoints, true));
                unitActorSelector
                    .Setup(s => s.CurrentUnit).Returns(actor.Object);
            }

            return new IntentSelector(unitActorSelector.Object);
        }
    }

}