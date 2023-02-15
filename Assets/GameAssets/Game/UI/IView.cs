using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public interface IView : IVisible
    {
        bool IsVisible { get; }
    }
}
