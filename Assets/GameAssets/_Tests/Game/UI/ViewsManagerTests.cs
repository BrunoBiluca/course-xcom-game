using NUnit.Framework;

namespace GameAssets.Tests
{
    public class ViewsManagerTests
    {
        [Test]
        public void Should_show_all_views_registered()
        {
            var view = new ViewMockBuilder().Build();

            var viewsManager = new ViewsGroup();
            viewsManager.Register(view);

            viewsManager.Show();

            Assert.That(view.IsVisible, Is.True);
        }

        [Test]
        public void Should_hide_all_views_registered()
        {
            var view = new ViewMockBuilder() { StartVisible = true }.Build();

            var viewsManager = new ViewsGroup();
            viewsManager.Register(view);

            viewsManager.Hide();

            Assert.That(view.IsVisible, Is.False);
        }
    }
}