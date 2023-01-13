using UnityEngine;
using UnityEngine.UI;

namespace GameAssets
{
    public class GameSceneLoaderButton : MonoBehaviour
    {
        [SerializeField] private GameSceneLoader gameSceneLoader;

        public void Awake()
        {
            if(!TryGetComponent(out Button button))
            {
                Debug.LogWarning("Button was not found in this gameobject");
                return;
            }

            button.onClick.AddListener(gameSceneLoader.Execute);
        }
    }
}
