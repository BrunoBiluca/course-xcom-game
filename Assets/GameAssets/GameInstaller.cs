using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class GameInstaller : Singleton<GameInstaller>
    {
        [SerializeField] private GameObject worldCursorRef;
        private IWorldCursor worldCursor;

        [SerializeField] private GridXZMono grid;

        protected override void OnAwake()
        {
            worldCursor = worldCursorRef.GetComponent<IWorldCursor>();

            foreach(var unit in FindObjectsOfType<UnitMono>())
            {
                unit.Setup(worldCursor, grid);
            }

            foreach(var unitSelection in FindObjectsOfType<UnitSelectionMono>())
            {
                unitSelection.Setup(worldCursor);
            }
        }
    }
}
