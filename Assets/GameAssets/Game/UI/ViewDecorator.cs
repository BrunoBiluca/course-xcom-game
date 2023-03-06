using UnityFoundation.Code;

namespace GameAssets
{
    public class ViewDecorator : IView
    {
        private readonly IGameObject refGameObject;

        public bool IsVisible => refGameObject.IsActiveInHierarchy;

        public bool StartVisible { get; set; }

        public ViewDecorator(IGameObject gameObject)
        {
            refGameObject = gameObject;
            refGameObject.SetActive(StartVisible);
        }

        public void Hide()
        {
            refGameObject.SetActive(false);
        }

        public void Show()
        {
            refGameObject.SetActive(true);
        }
    }
}
