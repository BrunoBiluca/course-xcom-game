using GameAssets.ActorSystem;
using Moq;
using UnityFoundation.ResourceManagement;

namespace GameAssets.Tests
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
        public APUnitActionSelection Build()
        {
            var unitActorSelector = new Mock<IActorSelector<IAPActor>>();

            if(isUnitSelected)
            {
                var actor = new Mock<IAPActor>();
                actor
                    .Setup(a => a.ActionPoints)
                    .Returns(new FiniteResourceManager(initialActionPoints, true));
                unitActorSelector
                    .Setup(s => s.CurrentUnitActor).Returns(actor.Object);
            }

            return new APUnitActionSelection(unitActorSelector.Object);
        }
    }

}