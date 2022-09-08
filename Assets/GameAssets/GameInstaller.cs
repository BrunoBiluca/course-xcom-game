using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Editor.Hierarchy;

namespace GameAssets
{
    public class GameInstaller : Singleton<GameInstaller>, IPrettyable
    {
        [SerializeField] private GameObject worldCursorRef;
        private IWorldCursor worldCursor;

        [SerializeField] private GridXZMono grid;

        public PrettyObject BePretty()
        {
            var installerColor = new Color(.38f, .35f, .06f);
            return new PrettyObject(false, installerColor, Color.white, gameObject);
        }

        protected override void OnAwake()
        {
            worldCursor = worldCursorRef.GetComponent<IWorldCursor>();
            grid.Setup();

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
