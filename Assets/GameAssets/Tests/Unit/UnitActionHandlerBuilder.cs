using Moq;

namespace GameAssets.Tests
{
    public sealed class UnitActionHandlerBuilder
    {
        private bool isUnitSelected = false;
        public UnitActionHandlerBuilder WithCurrentUnitSelected()
        {
            isUnitSelected = true;
            return this;
        }
        public APUnitActionSelection Build()
        {
            var unitActorSelector = new Mock<IActorSelector<IAPActor>>();

            if(isUnitSelected)
            {
                unitActorSelector
                    .Setup(s => s.CurrentUnitActor).Returns(new Mock<IAPActor>().Object);
            }

            return new APUnitActionSelection(unitActorSelector.Object);
        }
    }

}