using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAssets
{
    public class GameSceneLoader : MonoBehaviour
    {
        public event Action OnAllScenesLoaded;

        private bool areScenesLoaded;
        private bool areScenesUnloaded;

        [SerializeField] private SceneLoaderConfigSO configSO;

        private SceneLoaderConfig config;

        public void Awake()
        {
            config = configSO.Config;
            if(config.ExecuteOnAwake)
                Execute();
        }

        public void Execute()
        {
            areScenesLoaded = false;
            areScenesUnloaded = false;

            StartCoroutine(nameof(Load));
            StartCoroutine(nameof(Unload));
            StartCoroutine(nameof(WatingExecution));
        }

        private IEnumerator WatingExecution()
        {

            while(!areScenesLoaded || !areScenesUnloaded)
                yield return null;

            OnAllScenesLoaded?.Invoke();
        }

        private IEnumerator Load()
        {
            foreach(var sceneName in config.ScenesToLoad)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if(scene != null)
                    yield return LoadSceneFromHierarchy(scene);
                else
                    yield return LoadSceneAdditive(sceneName);

                Debug.Log($"{scene.name} was loaded.");
            }
            areScenesLoaded = true;
        }

        private IEnumerator LoadSceneFromHierarchy(Scene scene)
        {
            while(!scene.isLoaded)
                yield return null;
        }

        private IEnumerator LoadSceneAdditive(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while(!operation.isDone)
                yield return null;
        }

        private IEnumerator Unload()
        {
            foreach(var sceneName in config.ScenesToUnLoad)
            {
                var operation = SceneManager.UnloadSceneAsync(sceneName);
                while(!operation.isDone)
                    yield return null;
            }
            areScenesUnloaded = true;
        }
    }
}
