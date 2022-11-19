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
            var unitActorSelector = new Mock<IUnitActorSelector<IAPUnitActor>>();

            if(isUnitSelected)
            {
                unitActorSelector
                    .Setup(s => s.CurrentUnitActor).Returns(new Mock<IAPUnitActor>().Object);
            }

            return new APUnitActionSelection(unitActorSelector.Object);
        }
    }

}