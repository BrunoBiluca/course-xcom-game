using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class GameInstaller : Singleton<GameInstaller>
    {
        [SerializeField] private GameObject worldCursorRef;

        private IWorldCursor worldCursor;

        protected override void OnAwake()
        {
            worldCursor = worldCursorRef.GetComponent<IWorldCursor>();

            foreach(var unit in FindObjectsOfType<UnitMono>())
            {
                unit.Setup(worldCursor);
            }

            foreach(var unitSelection in FindObjectsOfType<UnitSelectionMono>())
            {
                unitSelection.Setup(worldCursor);
            }
        }
    }
}
