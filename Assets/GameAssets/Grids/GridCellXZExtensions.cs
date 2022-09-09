using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public static class GridCellXZExtensions
    {
        public static bool IsInRange<T>(this GridCellXZ<T> a, GridCellXZ<T> b, int range)
        {
            var x = Mathf.Abs(a.X - b.X);
            var z = Mathf.Abs(a.Z - b.Z);

            return x + z <= range;
        }
    }
}
