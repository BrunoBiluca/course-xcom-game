using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class SimpleUnit : MonoBehaviour, IUnit
    {
        public string Name => gameObject.name;

        public ITransform Transform { get; private set; }

        public void Awake()
        {
            Transform = new TransformDecorator(transform);
        }
    }
}
