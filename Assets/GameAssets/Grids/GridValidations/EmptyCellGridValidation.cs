using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class EmptyCellGridValidation<T> : IGridValidation<T>
    {
        public bool IsAvailable(GridCellXZ<T> cell)
        {
            return cell.IsEmpty();
        }
    }
}
