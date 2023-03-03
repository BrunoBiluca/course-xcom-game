using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityFoundation.Code;

namespace GameAssets
{
    public class GameSceneLoader : MonoBehaviour
    {
        public event Action OnAllScenesLoaded;

        private bool areScenesLoaded;
        private bool areScenesReloaded;
        private bool areScenesUnloaded;

        [SerializeField] private SceneLoaderConfigSO configSO;

        private SceneLoaderConfig Config => configSO.Config;
        private GameObject transitionObj;

        public void Awake()
        {
            if(Config.ExecuteOnAwake)
                Execute();
        }

        public void Execute()
        {
            areScenesLoaded = false;
            areScenesReloaded = false;
            areScenesUnloaded = false;

            if(configSO.TransitionPrefab != null)
            {
                transitionObj = Instantiate(configSO.TransitionPrefab);
                DontDestroyOnLoad(transitionObj);
            }

            AsyncProcessor.I.StartCoroutine(Load(Config.ScenesToLoad));
            AsyncProcessor.I.StartCoroutine(Unload(Config.ScenesToUnLoad));
            AsyncProcessor.I.StartCoroutine(Reload(Config.ScenesToReload));
            AsyncProcessor.I.StartCoroutine(WatingExecution());
        }

        private IEnumerator Reload(List<string> scenesToReload)
        {
            if(scenesToReload.Count == 0)
            {
                areScenesReloaded = true;
                yield break;
            }

            var activeScene = scenesToReload[0];
            var operation = SceneManager.LoadSceneAsync(activeScene);
            while(!operation.isDone)
                yield return null;

            yield return Unload(scenesToReload.Skip(1).ToList());
            yield return Load(scenesToReload.Skip(1).ToList());

            areScenesReloaded = true;
        }

        private IEnumerator WatingExecution()
        {
            while(!areScenesLoaded || !areScenesUnloaded || !areScenesReloaded)
                yield return null;

            if(transitionObj != null)
                Destroy(transitionObj);

            OnAllScenesLoaded?.Invoke();
        }

        private IEnumerator Load(List<string> scenes)
        {
            foreach(var sceneName in scenes)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if(scene.IsValid())
                    yield return LoadSceneFromHierarchy(scene);
                else
                    yield return LoadSceneAdditive(sceneName);

                Debug.Log($"{sceneName} was loaded.");
            }
            areScenesLoaded = true;
        }

        private IEnumerator LoadSceneFromHierarchy(Scene scene)
        {
            Debug.Log("Load scene from hierarchy: " + scene.name);
            while(!scene.isLoaded)
                yield return null;
        }

        private IEnumerator LoadSceneAdditive(string sceneName)
        {
            Debug.Log("Loading additivelly: " + sceneName);
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while(!operation.isDone)
                yield return null;
        }

        private IEnumerator Unload(List<string> scenes)
        {
            foreach(var sceneName in scenes)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if(!scene.IsValid())
                    continue;

                var operation = SceneManager.UnloadSceneAsync(sceneName);
                if(operation == null)
                    continue;

                while(!operation.isDone)
                    yield return null;
            }
            areScenesUnloaded = true;
        }
    }
}
