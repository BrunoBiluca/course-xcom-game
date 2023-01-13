using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class GameBinder : MonoBehaviour
    {
        public event Action OnBinderFinish;

        private readonly Dictionary<Type, object> container = new();

        public void Awake()
        {
            GetComponent<GameSceneLoader>().OnAllScenesLoaded += SetupBinder;
        }

        private void SetupBinder()
        {
            Debug.Log("Start GameBinder");
            Bind<IBilucaLogger>(UnityDebug.I);
            Bind<UnitWorldGridXZ>(FindObjectOfType<UnitWorldGridXZ>());
            Debug.Log("Finish GameBinder");

            OnBinderFinish?.Invoke();
        }

        public void Bind<T>(object reference)
        {
            container.Add(typeof(T), reference);
        }

        public T GetReference<T>()
        {
            return (T)container[typeof(T)];
        }
    }
}
