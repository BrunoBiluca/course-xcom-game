using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public static class GridCellXZExtensions
    {
        public static bool IsInRange<T>(this GridCellXZ<T> a, GridCellXZ<T> b, int range)
        {
            var x = Mathf.Abs(a.Position.X - b.Position.X);
            var z = Mathf.Abs(a.Position.Z - b.Position.Z);

            return x + z <= range;
        }
    }
}
