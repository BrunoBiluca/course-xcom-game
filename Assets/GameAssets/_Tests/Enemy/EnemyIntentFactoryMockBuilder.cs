using Moq;
using UnityEngine;

namespace GameAssets.Tests
{
    public sealed class EnemyIntentFactoryMockBuilder : MockBuilder<IEnemyActionIntentFactory>
    {
        protected override Mock<IEnemyActionIntentFactory> OnBuild()
        {
            var intentsFactory = new Mock<IEnemyActionIntentFactory>();
            intentsFactory
                .Setup(f => f.IntentMove(It.IsAny<ICharacterUnit>(), It.IsAny<Vector3>()))
                .Returns(new IntentMockBuilder() { ExecuteImediatly = true }.Build());

            intentsFactory
                .Setup(f => f.IntentShoot(It.IsAny<ICharacterUnit>(), It.IsAny<Vector3>()))
                .Returns(new IntentMockBuilder() { ExecuteImediatly = true }.Build());

            return intentsFactory;
        }
    }
}