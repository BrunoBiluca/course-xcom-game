using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class EnemyMono : MonoBehaviour
    {
        public ITransform Transform { get; private set; }

        public void Awake()
        {
            Transform = new TransformDecorator(transform);
        }
    }
}
