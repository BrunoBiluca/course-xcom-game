using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class UnityDebug : Singleton<UnityDebug>
    {
        public void Log(params string[] message)
        {
            Debug.Log(string.Join(" ", message));
        }
    }
}
