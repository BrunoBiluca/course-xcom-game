using UnityFoundation.Code;

namespace GameAssets
{
    public sealed class SimpleSelector
    {
        public Optional<ISelectable> Selected { get; private set; } = new();

        public void Select(ISelectable selectable)
        {
            Selected = Optional<ISelectable>.Some(selectable);
            Selected.Some(u => u.SetSelected(true));
        }

        public void Unselect()
        {
            Selected.Some(u => u.SetSelected(false));
            Selected = Optional<ISelectable>.None();
        }
    }
}