using UnityFoundation.Code;

namespace GameAssets
{
    public interface IVisibilityHandler
    {
        void Add(IGameObject gameObject);
        void Hide();
        void Show();
    }
}
