using UnityEngine;
using UnityFoundation.UI;

namespace GameAssets
{
    public class LoadSceneGameOverAction : MonoBehaviour, IGameOverAction
    {
        public string Name => "Reload";

        public void Execute()
        {
            GetComponent<GameSceneLoader>().Execute();
        }
    }
}
