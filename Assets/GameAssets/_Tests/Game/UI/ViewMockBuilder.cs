using Moq;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{
    public class ViewMockBuilder : MockBuilder<IView>
    {
        public bool StartVisible { get; set; } = false;
        protected override Mock<IView> OnBuild()
        {
            var view = new Mock<IView>();
            var viewIsVisible = StartVisible;
            view.Setup(v => v.Show()).Callback(() => viewIsVisible = true);
            view.Setup(v => v.Hide()).Callback(() => viewIsVisible = false);
            view.Setup(v => v.IsVisible).Returns(() => viewIsVisible);
            return view;
        }
    }
}