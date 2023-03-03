using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets
{
    [CreateAssetMenu(
        menuName = "Xcom Game/Scene Loader configuration",
        fileName = "scene_loader_config"
    )]
    public class SceneLoaderConfigSO : ScriptableObject
    {
        [field: SerializeField] public SceneLoaderConfig Config { get; private set; }
        [field: SerializeField] public GameObject TransitionPrefab { get; private set; }
    }

    [Serializable]
    public class SceneLoaderConfig
    {
        [field: SerializeField] public bool ExecuteOnAwake { get; private set; }
        [field: SerializeField] public List<string> ScenesToLoad { get; private set; }
        [field: SerializeField] public List<string> ScenesToReload { get; private set; }
        [field: SerializeField] public List<string> ScenesToUnLoad { get; private set; }
    }
}
