using Moq;
using UnityFoundation.CharacterSystem.ActorSystem;

namespace GameAssets.Tests
{
    public sealed class ActionMockBuilder : MockBuilder<IAction>
    {
        public bool IsExecutable { get; set; } = true;

        protected override Mock<IAction> OnBuild()
        {
            var action = new Mock<IAction>();

            if(IsExecutable)
                action.Setup(a => a.Execute()).Raises(a => a.OnFinishAction += null);
            else
                action.Setup(a => a.Execute()).Raises(a => a.OnCantExecuteAction += null);

            return action;
        }
    }
}