using Moq;
using UnityEngine;
using UnityFoundation.TestUtility;
using UnityFoundation.WorldCursors;

namespace GameAssets.Tests
{
    public class AreaAttackIntentFake : FakeBuilder<IGridIntent>
    {
        protected override IGridIntent OnBuild()
        {
            var character = new CharacterUnitMock()
                .WithPosition(Vector3.zero)
                .WithConfig(new UnitConfig(explosionRange: 1, areaAttackRange: 1))
                .Build();

            var characterSelector = new Mock<ICharacterSelector>();
            characterSelector.Setup(s => s.CurrentUnit).Returns(character);

            return new AreaAttackIntent(
                new ActionsConfig(),
                characterSelector.Object,
                new Mock<IWorldCursor>().Object
            );
        }
    }
}
