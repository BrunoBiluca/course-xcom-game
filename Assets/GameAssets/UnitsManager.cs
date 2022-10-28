using UnityEngine;

namespace GameAssets
{
    public class UnitsManager : MonoBehaviour
    {
        public UnitMono[] GetAllUnits()
        {
            return FindObjectsOfType<UnitMono>();
        }
    }
}
