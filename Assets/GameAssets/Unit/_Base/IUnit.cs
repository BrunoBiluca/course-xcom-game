using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    /// <summary>
    /// Base interface for placeable units in the grid
    /// </summary>
    public interface IUnit
    {
        string Name { get; }
        ITransform Transform { get; }
    }
}
