using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public interface IGridValidation<T>
    {
        bool IsAvailable(GridCellXZ<T> cell);
    }
}
