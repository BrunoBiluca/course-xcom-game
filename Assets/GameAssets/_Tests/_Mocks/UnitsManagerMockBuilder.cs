using Moq;
using UnityEngine;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{
    public class UnitsManagerMockBuilder : MockBuilder<IUnitsManager>
    {
        private Vector3 position;

        public UnitsManagerMockBuilder WithUnit(Vector3 position)
        {
            this.position = position;
            return this;
        }

        protected override Mock<IUnitsManager> OnBuild()
        {
            var unit = new CharacterUnitMock().WithPosition(position);

            var mock = new Mock<IUnitsManager>();
            mock.Setup(um => um.GetAllUnits()).Returns(new ICharacterUnit[] { unit.Build() });
            return mock;
        }
    }
}