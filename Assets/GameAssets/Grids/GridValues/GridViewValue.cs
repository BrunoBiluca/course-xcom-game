using UnityEngine;

namespace GameAssets
{
    public class GridViewValue
    {
        private readonly GameObject cellRef;
        private readonly Material cellMat;

        public GridViewValue(GameObject cellRef)
        {
            this.cellRef = cellRef;
            cellMat = cellRef.transform.Find("quad").GetComponent<Renderer>().material;
        }

        public void EnableCellRef()
        {
            cellRef.SetActive(true);
        }

        public void DisableCellRef()
        {
            cellRef.SetActive(false);
        }

        public void SetColor(Color color)
        {
            cellMat.color = color;
        }
    }
}
