using System;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class WorldCursorDebug : Singleton<WorldCursorDebug>
    {
        private WorldCursor worldCursor;
        private GameObject debugVisual;

        [field: SerializeField] private bool DebugMode { get; set; }

        public void Start()
        {
            worldCursor = WorldCursor.Instance;

            debugVisual = transform.Find("debug_visual").gameObject;
            debugVisual.SetActive(DebugMode);
        }

        public void Update()
        {
            if(worldCursor == null) return;

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