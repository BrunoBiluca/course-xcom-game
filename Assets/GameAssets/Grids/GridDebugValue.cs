using TMPro;
using UnityEngine;

namespace GameAssets
{
    public class GridDebugValue
    {
        private readonly GameObject cellRef;
        private readonly TextMeshPro text;

        public GridDebugValue(
            TextMeshPro text,
            GameObject cellRef
        )
        {
            this.text = text;
            this.cellRef = cellRef;
        }

        public void EnableCellRef()
        {
            cellRef.SetActive(true);
        }

        public void DisableCellRef()
        {
            cellRef.SetActive(false);
        }
    }
}
