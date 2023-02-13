using Moq;
using UnityEngine;
using UnityFoundation.TestUtility;
using UnityFoundation.WorldCursors;

namespace GameAssets.Tests
{
    public class MoveIntentFake : FakeBuilder<IGridIntent>
    {
        protected override IGridIntent OnBuild()
        {
            var character = new CharacterUnitMock()
                .WithPosition(Vector3.zero)
                .WithConfig(new UnitConfig(movementRange: 1))
                .Build();

            var characterSelector = new Mock<ICharacterSelector>();
            characterSelector.Setup(s => s.CurrentUnit).Returns(character);

            var worldCursor = new Mock<IWorldCursor>();

            return new MoveIntent(
                new ActionsConfig(),
                characterSelector.Object,
                worldCursor.Object
            );
        }
    }
}
