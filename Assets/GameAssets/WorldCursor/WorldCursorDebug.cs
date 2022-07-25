using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class WorldCursorDebug : Singleton<WorldCursorDebug>, IWorldCursor
    {
        private WorldCursor worldCursor;
        private GameObject debugVisual;

        [field: SerializeField] private bool DebugMode { get; set; }

        public Optional<Vector3> WorldPosition => worldCursor.WorldPosition;

        public void Start()
        {
            worldCursor = WorldCursor.Instance;

            debugVisual = transform.Find("debug_visual").gameObject;
            debugVisual.SetActive(DebugMode);
        }

        public void Update()
        {
            worldCursor.Update();

            if(DebugMode)
            {
                worldCursor.WorldPosition.Some(pos => {
                    debugVisual.SetActive(true);
                    transform.position = pos;
                })
                .OrElse(() => debugVisual.SetActive(false));
            }
        }
    }
}