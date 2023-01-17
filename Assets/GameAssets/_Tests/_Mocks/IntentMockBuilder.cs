using Moq;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{

    public sealed class IntentMockBuilder : MockBuilder<IAPIntent>
    {
        public bool ExecuteImediatly { get; set; }

        protected override Mock<IAPIntent> OnBuild()
        {

            var intent = new Mock<IAPIntent>();
            intent.Setup(i => i.ActionPointsCost).Returns(1);
            intent.Setup(i => i.ExecuteImmediatly).Returns(ExecuteImediatly);
            intent.Setup(i => i.Create()).Returns(new ActionMockBuilder().Build());
            return intent;
        }
    }
}