using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAssets
{
    public class GameSceneManager : MonoBehaviour
    {
        public event Action OnAllScenesLoaded;

        [field: SerializeField] public List<string> ScenesToLoad { get; private set; }

        private void Awake()
        {
            StartCoroutine(nameof(Loading));
        }

        private IEnumerator Loading()
        {
            foreach(var name in ScenesToLoad)
            {
                var scene = SceneManager.GetSceneByName(name);
                while(!scene.isLoaded)
                    yield return null;

                Debug.Log($"{scene.name} was loaded.");
            }

            OnAllScenesLoaded?.Invoke();
        }
    }
}
