using Moq;
using UnityEngine;
using UnityFoundation.TestUtility;
using UnityFoundation.WorldCursors;

namespace GameAssets.Tests
{
    public class ShootIntentFake : FakeBuilder<IGridIntent>
    {
        protected override IGridIntent OnBuild()
        {
            var character = new CharacterUnitMock()
                .WithPosition(Vector3.zero)
                .WithConfig(new UnitConfig(shootRange: 2))
                .Build();

            var characterSelector = new Mock<ICharacterSelector>();
            characterSelector.Setup(s => s.CurrentUnit).Returns(character);

            var worldCursor = new Mock<IWorldCursor>();

            return new ShootIntent(
                new ActionsConfig(),
                characterSelector.Object,
                worldCursor.Object
            );
        }
    }
}
