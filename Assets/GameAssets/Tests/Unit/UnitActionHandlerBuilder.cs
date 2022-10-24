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
        public UnitActionHandler Build()
        {
            var unitActorSelector = new Mock<IUnitActorSelector>();

            if(isUnitSelected)
            {
                unitActorSelector
                    .Setup(s => s.CurrentUnitActor).Returns(new Mock<IUnitActor>().Object);
            }

            return new UnitActionHandler(unitActorSelector.Object);
        }
    }

}